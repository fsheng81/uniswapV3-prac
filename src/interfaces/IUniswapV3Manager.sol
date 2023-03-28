// SPDX-License-Identifier: GPL-2.0-or-later
pragma solidity ^0.8.0;

interface IUniswapV3Manager {
    struct MintParams {
        address tokenA;
        address tokenB;
        uint24 tickSpacing;
        int24 lowerTick;
        int24 upperTick;
        uint256 amount0Desired; /** 期望输入的amount0 */
        uint256 amount1Desired; /** 期望输入的amount1 */
        uint256 amount0Min; /** 校验最少应该输入的amount0 */
        uint256 amount1Min;
    }

    struct SwapSingleParams {
        address tokenIn;
        address tokenOut;
        uint24 tickSpacing;
        uint256 amountIn;
        uint160 sqrtPriceLimitX96; /** 最极限的交易价格 */
    }

    struct SwapParams {
        bytes path;
        address recipient;
        uint256 amountIn;
        uint256 minAmountOut; /** 交易后，最少的 amountOut */
    }

    struct SwapCallbackData {
        bytes path;
        address payer;
    }
}
