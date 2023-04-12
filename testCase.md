说明所有的测试用例：

默认 tickSpacing = 60



forge test -vvv --match-path test/UniswapV3Pool.t.sol --match-contract UniswapV3PoolTest --match-test "testMintInRange"

forge test -vvvv --match-path test/UniswapV3Pool.t.sol --match-contract UniswapV3PoolTest > testlog/testPool



## mint()

### 单次mint()

#### case1 in range

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

输出

```
amount:[0.987078348444137445 ether, 5000 ether]
liquidity: 
sqrtPriceX96: sqrtP(5000),
tick: tick(5000),
ticks:[tick_low, tick_up]
```

池子的当前tick不受tickSpacing影响。

交易后的token数目：userID -> pool。

#### case2 below range

```
现价：5000
tick的价格范围：[4000, 4996]
输入amount：[1 ether, 5000 ether].
```

如果是4996 -> 4999呢？4999.9999就变成5000

输出

```
amount:[0 ether, 4999.999999999999999994 ether]
liquidity: 
sqrtPriceX96: sqrtP(5000),
tick: tick(5000),
ticks:[tick_low, tick_up]
```

#### case3 above range

```
现价：5000
tick的价格范围：[5001, 6250]
输入amount：[1 ether, 5000 ether].
```

输出

```
amount:[1 ether, 0 ether]
liquidity: 
sqrtPriceX96: sqrtP(5000),
tick: tick(5000),
ticks:[tick_low, tick_up]
```

#### case4 overLapping range

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

```
tick的价格范围：[4000, 6250]
输入amount：[0.8 ether, 4000 ether].
```

输出

```
amount:[1.782930003452677700 ether, 8999.999999999999999997 ether]
liquidity: 
sqrtPriceX96: sqrtP(5000),
tick: tick(5000),
ticks:[tick_low, tick_up]
```

#### case testMintInvalidTickRangeLower

lowTick超过了-887272的范围

#### case testMintInvalidTickRangeUpper

upTick超过了887272的范围

#### case testMintZeroLiquidity

流动性为0

#### case testMintInsufficientTokenBalance

用户没有足够的token输入

## swap()

#### case1 BuyETHOnePriceRange

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

```
发生交易 swap 42 USDC TO weth
```

输出

```
-0.008396774627565324 ether,
sqrtPriceX96: 5604429046402228950611610935846
tick: 85183
fees: 0 27727650748765949686643356806934465
```

交易后的tick依旧是没有按照tickSpacing来计算？

#### case2 BuyETHTwoPriceRange

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

```
发生交易 swap 42 USDC TO weth
```

输出

```
-0.008398387004109300 ether,
sqrtPriceX96: 5603353071940421471240346849555
tick: 85183
fees: 0 27727650748765949686643356806934465
```

输出的weth的数目并不一样

#### case3 BuyETHConsecutivePriceRange

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
tick的价格范围：[5500, 6250]
输入amount：[1 ether, 5000 ether].
```

```
发生交易 swap 1000 USDC TO weth
limit price: sqrtP(6106),
```

输出

```
-1.806151062659754716 ether,
9908.332401339128822272 ether

sqrtPriceX96: 6190959796047061453084569894912
tick: 85183
fees: 0 27727650748765949686643356806934465
```

输出的weth的数目并不一样







## fee

#### case1 burn

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

```
tick的价格范围：[4000, 6250]
burn amount：按照全部liquidity
```

输出

```
amount:[0.987078348444137445 ether, 5000 ether]
liquidity: 0
sqrtPriceX96: sqrtP(5000),
tick: tick(5000),
ticks: burn了之后，应该把initialize改为false
```

#### case2 burnPartialy

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

```
tick的价格范围：[4000, 6250]
burn amount：[liquidity / 2]
```

输出

```
amount:[0.493539174222068722 ether, 2499.999999999999999997 ether]
liquidity: liquidity / 2  + 1
sqrtPriceX96: sqrtP(5000),
tick: tick(5000),
ticks: burn了之后，应该把initialize改为false
```

开始有+1的操作

针对liquidityGross 和 liquidityNet也是如此。

#### case3 collect

```
现价：5000
tick的价格范围：[4545, 5500]
输入amount：[1 ether, 5000 ether].
```

```
发生交易 swap 42 USDC TO weth
tick的价格范围：[4000, 6250]
burn amount：[liquidity / 2]
```

输出

```

```

#### case4

#### case flash



