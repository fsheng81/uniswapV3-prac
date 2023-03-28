// SPDX-License-Identifier: GPL-2.0-or-later
pragma solidity ^0.8.14;

// 采用定点数：Q64.96
// 十六进制与实际小数的转换关系为：
// Q64.96 = uint160 / (1 << 96)
library FixedPoint96 {
    uint8 internal constant RESOLUTION = 96;
    uint256 internal constant Q96 = 2**96;
}
