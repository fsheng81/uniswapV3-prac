// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

import "./LiquidityMath.sol";
import "./Math.sol";

// tick 是用于方便表达/搜索 price 的值
// 但是也承担记录 流动性变化的功能

// ---low---up----
// 当注入流动性时，调用ticks.update()
// bool upper会判断是 lowTIck 还是 upperTick
// 如果是 upTick，此次注入的流动性是 -Delta

// 当价格从下而上进入区间，zeroForOne == false
// cross low 时 加上了Delta，
// cross up  时 加上了此时 -Delta

// 当价格从上而下进入区间时，zeroForOne == true
// cross low 时 减去了 +Delta
// cross up  时 减去了 -Delta

library Tick {
    struct Info {
        bool initialized;
        uint128 liquidityGross; // 该 tick 的总liquidity
        int128 liquidityNet; // 当curPrice经过tick时的liquidity变化量
    }

    function update(
        mapping(int24 => Tick.Info) storage self, /** storage引用 只有library有这个用法 */
        int24 tick,
        int128 liquidityDelta, /** 正负号代表注入/移除流动性 */
        bool upper
    ) internal returns (bool flipped) {
        Tick.Info storage tickInfo = self[tick];

        uint128 liquidityBefore = tickInfo.liquidityGross;
        uint128 liquidityAfter = LiquidityMath.addLiquidity(
            liquidityBefore,
            liquidityDelta
        );

        // 影响 bitMap
        flipped = (liquidityAfter == 0) != (liquidityBefore == 0);

        if (liquidityBefore == 0) {
            tickInfo.initialized = true;
        }

        tickInfo.liquidityGross = liquidityAfter;
        tickInfo.liquidityNet = upper
            ? int128(int256(tickInfo.liquidityNet) - liquidityDelta)
            : int128(int256(tickInfo.liquidityNet) + liquidityDelta);
    }

    function cross(mapping(int24 => Tick.Info) storage self, int24 tick)
        internal
        view
        returns (int128 liquidityDelta)
    {
        Tick.Info storage info = self[tick];
        liquidityDelta = info.liquidityNet;
    }
}
