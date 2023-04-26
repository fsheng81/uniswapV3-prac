// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

import "./LiquidityMath.sol";
import "./MathAmount.sol";

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
        uint256 feeGrowthOutside0X128;
        uint256 feeGrowthOutside1X128;
    }

    function update(
        mapping(int24 => Tick.Info) storage self, /** storage引用 只有library有这个用法 */
        int24 tick,
        int24 currentTick,
        int128 liquidityDelta, /** 正负号代表注入/移除流动性 */
        uint256 feeGrowthGlobal0X128,
        uint256 feeGrowthGlobal1X128,
        bool upper /** 是否是position的upperTick */
    ) internal returns (bool flipped) {
        Tick.Info storage tickInfo = self[tick];

        uint128 liquidityBefore = tickInfo.liquidityGross;
        uint128 liquidityAfter = LiquidityMath.addLiquidity(liquidityBefore, liquidityDelta);

        // flipped: 是否影响bitMap
        flipped = (liquidityAfter == 0) != (liquidityBefore == 0);

        if (liquidityBefore == 0) {
            // 见outside定义，基于cross()的计算方式，来设置
            if (tick <= currentTick) {
                tickInfo.feeGrowthOutside0X128 = feeGrowthGlobal0X128;
                tickInfo.feeGrowthOutside1X128 = feeGrowthGlobal1X128;
            }
            tickInfo.initialized = true;
        }
        // todo: if liquidityAfter == 0 , initialize = false.

        // 滑过lowTick时，会根据 liquidityNet 增加这个流动性Delta，
        // 滑过 upTick时，会根据 liquidityNet 减少这个流动性Delta
        tickInfo.liquidityGross = liquidityAfter;
        tickInfo.liquidityNet = upper
            ? int128(int256(tickInfo.liquidityNet) - liquidityDelta)
            : int128(int256(tickInfo.liquidityNet) + liquidityDelta);
    }

    function cross(
        mapping(int24 => Tick.Info) storage self,
        int24 tick,
        uint256 feeGrowthGlobal0X128,
        uint256 feeGrowthGlobal1X128
    ) internal returns (int128 liquidityDelta) {
        Tick.Info storage info = self[tick];

        // if left -> right cross 
        // global - 上一次的outside(那上一次一定是 right -> left)
        // 此时是获得了 距离上一次cross() 这一段时间中的所有增量。
        info.feeGrowthOutside0X128 = feeGrowthGlobal0X128 - info.feeGrowthOutside0X128;
        info.feeGrowthOutside1X128 = feeGrowthGlobal1X128 - info.feeGrowthOutside1X128;

        // 返回此时的全局流动性增量
        liquidityDelta = info.liquidityNet;
    }

    function getFeeGrowthInside(
        mapping(int24 => Tick.Info) storage self,
        int24 lowerTick_,
        int24 upperTick_,
        int24 currentTick,
        uint256 feeGrowthGlobal0X128,
        uint256 feeGrowthGlobal1X128
    )
        internal
        view
        returns (uint256 feeGrowthInside0X128, uint256 feeGrowthInside1X128)
    {
        Tick.Info storage lowerTick = self[lowerTick_];
        Tick.Info storage upperTick = self[upperTick_];

        // 完全没有必要这么复杂......
        uint256 feeGrowthBelow0X128;
        uint256 feeGrowthBelow1X128;
        if (currentTick >= lowerTick_) {
            feeGrowthBelow0X128 = lowerTick.feeGrowthOutside0X128;
            feeGrowthBelow1X128 = lowerTick.feeGrowthOutside1X128;
        } else {
            feeGrowthBelow0X128 = feeGrowthGlobal0X128 - lowerTick.feeGrowthOutside0X128;
            feeGrowthBelow1X128 = feeGrowthGlobal1X128 - lowerTick.feeGrowthOutside1X128;
        }

        uint256 feeGrowthAbove0X128;
        uint256 feeGrowthAbove1X128;
        if (currentTick < upperTick_) {
            feeGrowthAbove0X128 = upperTick.feeGrowthOutside0X128;
            feeGrowthAbove1X128 = upperTick.feeGrowthOutside1X128;
        } else {
            feeGrowthAbove0X128 = feeGrowthGlobal0X128 - upperTick.feeGrowthOutside0X128;
            feeGrowthAbove1X128 = feeGrowthGlobal1X128 - upperTick.feeGrowthOutside1X128;
        }

        // 记录中很多是0值。
        // inside = global - below - above.

        // inside: position [lowTick, upTick]
        // 
        feeGrowthInside0X128 = feeGrowthGlobal0X128 - feeGrowthBelow0X128 - feeGrowthAbove0X128;
        feeGrowthInside1X128 =
            feeGrowthGlobal1X128 -
            feeGrowthBelow1X128 -
            feeGrowthAbove1X128;
    }
}
