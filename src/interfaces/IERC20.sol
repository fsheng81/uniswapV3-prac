// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

/** 只用一部分 */
interface IERC20 {
    function approve(address, uint256) external;

    function balanceOf(address) external returns (uint256);

    function transfer(address, uint256) external;

    function transferFrom(address, address, uint256) external;
}