// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

import "../src/UniswapV3Pool.sol";
import "../src/interfaces/IERC20.sol";

// 实现四个函数 mint() swap() mintCallback() swapCallback()

contract UniswapV3Manager {
    function mint(
        address poolAddress_,
        int24 lowerTick,
        int24 upperTick,
        uint128 liquidity,
        bytes calldata data
    ) public returns (uint256, uint256) {
        return
            UniswapV3Pool(poolAddress_).mint(
                msg.sender,
                lowerTick,
                upperTick,
                liquidity,
                data
            );
    }

    function swap(address poolAddress_, bytes calldata data)
        public
        returns (int256, int256)
    {
        return UniswapV3Pool(poolAddress_).swap(msg.sender, data);
    }

    function uniswapV3MintCallback(
        uint256 amount0,
        uint256 amount1,
        bytes calldata data
    ) public {
        UniswapV3Pool.CallbackData memory extra = abi.decode(
            data,
            (UniswapV3Pool.CallbackData)
        );

        // 讲token 从 extra.payer账户 转移到 msg.sender账户
        // pay是data中记录的地址，由外部提供
        // msg.sender 是 pool 地址
        IERC20(extra.token0).transferFrom(extra.payer, msg.sender, amount0);
        IERC20(extra.token1).transferFrom(extra.payer, msg.sender, amount1);
    }

    function uniswapV3SwapCallback(
        int256 amount0,
        int256 amount1,
        bytes calldata data
    ) public {
        UniswapV3Pool.CallbackData memory extra = abi.decode(
            data,
            (UniswapV3Pool.CallbackData)
        );
        // amount > 0 : 由外部向pool转账
        if (amount0 > 0) {
            IERC20(extra.token0).transferFrom(
                extra.payer,
                msg.sender,
                uint256(amount0)
            );
        }

        if (amount1 > 0) {
            IERC20(extra.token1).transferFrom(
                extra.payer,
                msg.sender,
                uint256(amount1)
            );
        }
    }
}
