// SPDX-License-Identifier: GPL-2.0-or-later
pragma solidity ^0.8.0;

import "prb-math/Common.sol"; // github库
import "./FixedPoint96.sol";

// 功能：mint()时
// 通过输入的amountX 和 价格的变化Delta值 计算流动性的大小
// 可以把流动性看做是一种阻力，
// 在swap时，当输入了amount后，阻止价格移动的阻力。
// 流动性越大，价格移动的越缓慢。

// 注入流动性时，current price不会发生变化。
// 存在三种情况：

// 1. curPrice < lowPrice < upPrice:
// 当curPrice移动到区间的过程中，价格上升，
// swap用户支出token1换取池子中的token0
// 所以面对LP，池子的需求是提供token0
// 因此LP 只需要对池子注入 token0 即可

// 另一种理解是 cur 进入池子时 是 lowPrice. 
// 这个点对应的曲线图中，是 局部池子 中token1 已经被换完了
// 所以此时 局部池子 只需要 token0
// 并能够满足 从low->up的过程中，逐步把这些 token0 换为 token1
// 就像势能与动能的不断切换一样。

// 2. lowPrice < curPrice < upPrice
// 此时需要两种token
// 如果是 lowPrice <- curPrice：价格下降 池子需求 token1
// 如果是 curPrice -> upPrice：价格上升 池子需求 token0
// 为了保证 两边都能够完成转换：到达边界价格位置
// 应该取 小的流动性

// 3. lowPrice < upPrice < curPrice
// 此时价格下降，池子需求token1，因此LP注入token1

library LiquidityMath {
    /// $L = \frac{\Delta x \sqrt{P_u} \sqrt{P_l}}{\Delta \sqrt{P}}$
    function getLiquidityForAmount0(
        uint160 sqrtPriceAX96,
        uint160 sqrtPriceBX96,
        uint256 amount0
    ) internal pure returns (uint128 liquidity) {
        if (sqrtPriceAX96 > sqrtPriceBX96)
            (sqrtPriceAX96, sqrtPriceBX96) = (sqrtPriceBX96, sqrtPriceAX96);

        // sqrtPriceAX96 * sqrtPriceBX96 / FixedPoint96.Q96
        // mulDiv() in PRB-MATH
        uint256 intermediate = mulDiv(
            sqrtPriceAX96,
            sqrtPriceBX96,
            FixedPoint96.Q96
        );
        liquidity = uint128(
            mulDiv(amount0, intermediate, sqrtPriceBX96 - sqrtPriceAX96)
        );
    }

    /// $L = \frac{\Delta y}{\Delta \sqrt{P}}$
    function getLiquidityForAmount1(
        uint160 sqrtPriceAX96,
        uint160 sqrtPriceBX96,
        uint256 amount1
    ) internal pure returns (uint128 liquidity) {
        if (sqrtPriceAX96 > sqrtPriceBX96)
            (sqrtPriceAX96, sqrtPriceBX96) = (sqrtPriceBX96, sqrtPriceAX96);

        liquidity = uint128(
            mulDiv(
                amount1,
                FixedPoint96.Q96,
                sqrtPriceBX96 - sqrtPriceAX96
            )
        );
    }

    // 入口
    function getLiquidityForAmounts(
        uint160 sqrtPriceX96,
        uint160 sqrtPriceAX96,
        uint160 sqrtPriceBX96,
        uint256 amount0,
        uint256 amount1
    ) internal pure returns (uint128 liquidity) {
        if (sqrtPriceAX96 > sqrtPriceBX96)
            (sqrtPriceAX96, sqrtPriceBX96) = (sqrtPriceBX96, sqrtPriceAX96);

        if (sqrtPriceX96 <= sqrtPriceAX96) {
            liquidity = getLiquidityForAmount0(
                sqrtPriceAX96,
                sqrtPriceBX96,
                amount0
            );
        } else if (sqrtPriceX96 <= sqrtPriceBX96) {
            uint128 liquidity0 = getLiquidityForAmount0(
                sqrtPriceX96,
                sqrtPriceBX96,
                amount0
            );
            uint128 liquidity1 = getLiquidityForAmount1(
                sqrtPriceAX96,
                sqrtPriceX96,
                amount1
            );

            liquidity = liquidity0 < liquidity1 ? liquidity0 : liquidity1;
        } else {
            liquidity = getLiquidityForAmount1(
                sqrtPriceAX96,
                sqrtPriceBX96,
                amount1
            );
        }
    }

    // 考虑到负数是补码格式，所以必须显示转换为正数
    function addLiquidity(uint128 x, int128 y)
        internal
        pure
        returns (uint128 z)
    {
        if (y < 0) {
            z = x - uint128(-y);
        } else {
            z = x + uint128(y);
        }
    }
}
