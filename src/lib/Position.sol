// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

// [owner, uptick, lowTick] -> [liquidity]
// owner 都是 manager
library Position {
    struct Info {
        uint128 liquidity;
    }

    function get(
        mapping(bytes32 => Info) storage self,
        address owner,
        int24 lowerTick,
        int24 upperTick
    ) internal view returns (Position.Info storage position) {
        position = self[
            /** 组装一个bytes32 */
            keccak256(abi.encodePacked(owner, lowerTick, upperTick))
        ];
    }

    function update(
        Info storage self,
        uint128 liquidityDelta
    ) internal {
        uint128 liquidityBefore = self.liquidity;
        uint128 liquidityAfter = liquidityBefore + liquidityDelta;

        self.liquidity = liquidityAfter;
    }
}
