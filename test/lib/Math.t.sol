// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.0;

import "forge-std/Test.sol";
import "../../src/lib/Math.sol";
import "../../src/lib/TickMath.sol";

// 测试 Math.sol 和 TickMath.sol
// 
contract MathTest is Test {
    function testCalcAmount0Delta() public {
        int256 amount0 = Math.calcAmount0Delta(
            TickMath.getSqrtRatioAtTick(85176),
            TickMath.getSqrtRatioAtTick(86129),
            int128(1517882343751509868544)
        );
        assertEq(TickMath.getSqrtRatioAtTick(85176), 5602223755577321903022134995689);
        assertEq(TickMath.getSqrtRatioAtTick(86129), 5875617940067453351001625213169);
        assertEq(amount0, 0.998833192822975409 ether);
    }

    function testCalcAmount1Delta() public {
        int256 amount1 = Math.calcAmount1Delta(
            TickMath.getSqrtRatioAtTick(84222),
            TickMath.getSqrtRatioAtTick(85176),
            int128(1517882343751509868544)
        );
        assertEq(TickMath.getSqrtRatioAtTick(85176), 5602223755577321903022134995689);
        assertEq(TickMath.getSqrtRatioAtTick(86129), 5875617940067453351001625213169);
        assertEq(amount1, 4999.187247111820044641 ether);
    }

    function testCalcAmount0DeltaNegative() public {
        int256 amount0 = Math.calcAmount0Delta(
            TickMath.getSqrtRatioAtTick(85176),
            TickMath.getSqrtRatioAtTick(86129),
            int128(-1517882343751509868544)
        );

        assertEq(amount0, -0.998833192822975408 ether);
    }

    function testCalcAmount1DeltaNegative() public {
        int256 amount1 = Math.calcAmount1Delta(
            TickMath.getSqrtRatioAtTick(84222),
            TickMath.getSqrtRatioAtTick(85176),
            int128(-1517882343751509868544)
        );

        assertEq(amount1, -4999.187247111820044640 ether);
    }
}
