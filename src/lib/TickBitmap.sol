// SPDX-License-Identifier: BUSL-1.1
pragma solidity >=0.5.0;

import "./BitMath.sol";

// tickMap 记录 int24 的所有 tick 是否激活，
// 采用 map(int16 => uint256) 的形式
// 每一个word有 256个bit位，0/1 正好表示 对应的 int8 bitPos的tick
library TickBitmap {
    function position(int24 tick)
        private
        pure
        returns (int16 wordPos, uint8 bitPos)
    {
        wordPos = int16(tick >> 8);
        bitPos = uint8(uint24(tick % 256));
    }

    function flipTick(
        mapping(int16 => uint256) storage self,
        int24 tick,
        int24 tickSpacing
    ) internal {
        require(tick % tickSpacing == 0); // 目前tickSpacing认为是1
        (int16 wordPos, uint8 bitPos) = position(tick / tickSpacing);
        uint256 mask = 1 << bitPos; // 对应bitPos位为1
        self[wordPos] ^= mask; // 异或操作 bitPos位取反
    }

    // 找到word中的下一个激活的tick
    // 问题：如果不在下一个word中呢？tick负号问题
    function nextInitializedTickWithinOneWord(
        mapping(int16 => uint256) storage self,
        int24 tick,
        int24 tickSpacing,
        bool lte
    ) internal view returns (int24 next, bool initialized) {

        // "/" 除法在负数的情况下，需要--。 -10 / 3 = 
        int24 compressed = tick / tickSpacing;
        if (tick < 0 && tick % tickSpacing != 0) compressed--;

        if (lte) {
            // 找一个比bitPos小的tick
            (int16 wordPos, uint8 bitPos) = position(compressed);

            // 00000111111 第一个1是bitPos位
            // 所以masked就是过滤掉了除了bitPos左边的所有比它大的tick
            uint256 mask = (1 << bitPos) - 1 + (1 << bitPos);
            uint256 masked = self[wordPos] & mask;

            // 如果 masked==0 那么同一个word中没有满足条件的激活tick了
            initialized = masked != 0;

            // bitPos 到 masked的最近一个1的距离 = delta
            // 再转换后才能参与 int24 计算
            next = initialized
                ? (compressed -
                    int24(uint24(bitPos - BitMath.mostSignificantBit(masked)))) * tickSpacing
                : (compressed - int24(uint24(bitPos))) * tickSpacing;
        } else {
            // 价格上升
            (int16 wordPos, uint8 bitPos) = position(compressed + 1);
            uint256 mask = ~((1 << bitPos) - 1);
            uint256 masked = self[wordPos] & mask;

            initialized = masked != 0;
            next = initialized
                ? (compressed +
                    1 + int24(uint24((BitMath.leastSignificantBit(masked) - bitPos)))) * tickSpacing
                : (compressed + 1 + int24(uint24((type(uint8).max - bitPos)))) * tickSpacing;
        }
    }
}
