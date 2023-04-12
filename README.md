uniswapV3-prac to understand the framework of uniswapV3.

dev2：仅包括最基础的实现，单用户的mint()，不跨tick的swap()，不涉及solidity复杂的计算，也不涉及手续费的收取。以token0 = WETH, token1 = USDC为例，事先用Python实现计算功能，了解基本框架。

dev3：增加了工厂类、solidity的数学计算、多个池子计算、跨tick交易。不涉及交易费用、预言机等。

dev4：增加交易费用、预言机。没有考虑tickSpacing计算过程中跨word。

结构为：

```bash
|- scripts # 部署
|- src # 源码
	|- interface
	|- lib # 为了保证 pool 合约的长度，把其他功能用 library 形式实现
	|- pool.sol # 交易池子的核心功能
	|- manager.sol # 用户通过manager调用pool中的函数。
|- test # 单元测试用例
|- ui # React实现的UI界面
```

## 说明

可以把流动性看做是一种阻力，在swap时，当输入了amount后，阻止价格移动的阻力。流动性越大，价格移动的越缓慢。

流动性，价格差，token量，总共有三个变量，相互的求解方式分别在 liquidityMath.sol和 Math.sol文件中



## 部署

``` bash
# 添加 libs 依赖
forge install foundry-rs/forge-std
forge install Rari-Capital/solmate
forge install GNSPS/solidity-bytes-utils
forge install paulrberg/prb-math
forge install abdk-consulting/abdk-libraries-solidity
forge install OpenZeppelin/openzeppelin-contracts

anvil # 另一个bash
forge test # 执行测试用例
forge build
# 部署脚本：PRIVATE_KEY 为 anvil生成的第一个账户秘钥
forge script scripts/DeployDevelopment.s.sol --broadcast --fork-url http://localhost:8545 --private-key $PRIVATE_KEY

forge script scripts/DeployDevelopment.s.sol --broadcast --fork-url http://localhost:8545 --private-key 0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80

# 部署(参考)
forge create <filePath>:<ContractName> --private-key=$PRIV_KEY

# 交易

```
为了解决vscode的报错，新增remappings.txt

部署OK：

```
== Logs ==
  WETH address 0x5FbDB2315678afecb367f032d93F642f64180aa3
  USDC address 0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512
  Pool address 0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0
  Manager address 0xCf7Ed3AccA5a467e9e704C703E8D87F634fB0Fc9

ONCHAIN EXECUTION COMPLETE & SUCCESSFUL.
Total Paid: 0.01060162056326995 ETH (2743235 gas * avg 3.831132733 gwei)

Transactions saved to: /home/fs/code/git-repo/uniswapV3-prac/broadcast/DeployDevelopment.s.sol/31337/run-latest.json
```

查询余额：

```
cast --to-dec 0x00000000000000000000000000000000000000000000011153ce5e56cf880000| cast --from-wei
```

`out: 5042.000000000000000000`

查询池子slot0信息：

````
cast call POOL_ADDRESS "slot0()"| xargs cast --abi-decode "a()(uint160,int24)"

cast call 0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0 "slot0()"| xargs cast --abi-decode "a()(uint160,int24)"
````

`5602277097478614198912276234240`
`85176`



#### UI设置：
设置app.js中的合约地址，并在浏览器中安装metamask插件。
`npm install -g yarn`

执行 `cd ui & yarn & yarn start`
先执行yarn来检查packages
自动跳转 http://localhost:3000/
参考：https://github.com/facebook/create-react-app
App.js执行components的组件，组件最后执行content中的metaMask.js



从dev3开始，在yarn start之前，需要

```bash
source .envrc
make deploy
```

结果：

```
== Logs ==
  WETH address 0x5FbDB2315678afecb367f032d93F642f64180aa3
  UNI address 0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0
  USDC address 0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512
  USDT address 0xDc64a140Aa3E981100a9becA4E685f962f0cF6C9
  WBTC address 0xCf7Ed3AccA5a467e9e704C703E8D87F634fB0Fc9
  Factory address 0x5FC8d32690cc91D4c39d9d3abcBD16989F875707
  Manager address 0x0165878A594ca255338adfa4d48449f69242Eb8F
  Quoter address 0xa513E6E4b8f2a923D98304ec87F64353C4D5C853
  USDT/USDC address 0x1eb6Cb8cdA2035356113362F5636a65038544C75
  WBTC/USDT address 0x469070001aD1523f36290c82eE5001eD142FC495
  WETH/UNI address 0xE7d90366732CE294C0867aBAb7FbB9bE17511145
  WETH/USDC address 0xBe9A08dFB3386a22F8Dd9E28f757d3AE44571ae9
```

再执行UI。

## 排查报错

### foundry版本问题

```
Error:

the application panicked (crashed).
Message:  assertion failed: `(left == right)`
  left: `20`,
 right: `32`
Location: /home/runner/.cargo/registry/src/github.com-1ecc6299db9ec823/generic-array-0.14.6/src/lib.rs:565

This is a bug. Consider reporting it at https://github.com/foundry-rs/foundry

Backtrace omitted. Run with RUST_BACKTRACE=1 environment variable to display it.
Run with RUST_BACKTRACE=full to include source snippets.
已放弃 (核心已转储)
```

本地的 foundry 版本有问题。
替换版本：
```foundryup --version nightly-94777647f6ea5d34572a1b15c9b57e35b8c77b41 ```

https://github.com/foundry-rs/foundry/tree/master/foundryup
https://github.com/foundry-rs/foundry/tree/nightly-94777647f6ea5d34572a1b15c9b57e35b8c77b41


## prb-math问题
remapping.txt中含有一下内容，导致编译失败，
所以在自己的remapping.txt中也添加。
```
src/=src/
```
注意到PRB-MATH的整改，所以mulDiv()函数不用 PRBMATH.mulDiv()

### add liquidity fail in metamask

一直在Loading pairs...界面没有跳转

应该使用makefile来部署合约，再启动UI。

```
source .envrc
make deploy
```

### yarn UI 报错

报错内容：

```
Compiled with problems:

ERROR in ./src/lib/pathFinder.js 3:0-39

Module not found: Error: Can't resolve 'ngraph.graph' in '/home/fs/code/git-repo/uniswapV3-prac/ui/src/lib'


ERROR in ./src/lib/pathFinder.js 4:0-31

Module not found: Error: Can't resolve 'ngraph.path' in '/home/fs/code/git-repo/uniswapV3-prac/ui/src/lib'
```



yarn add ngraph.graph

yarn add ngraph.path

### 找不到 jsbi.ts

Error: ENOENT: no such file or directory, open '/home/fs/code/git-repo/uniswapV3-prac/ui/node_modules/jsbi/lib/jsbi.ts'

来源：uniswapV3-sdk在计算的时候需要js的bigInt库。

```
Compiled with warnings.

Failed to parse source map from '/home/fs/code/git-repo/uniswapV3-prac/ui/node_modules/jsbi/lib/jsbi.ts' file: Error: ENOENT: no such file or directory, open '/home/fs/code/git-repo/uniswapV3-prac/ui/node_modules/jsbi/lib/jsbi.ts'

Search for the keywords to learn more about each warning.
To ignore, add // eslint-disable-next-line to the line before.

WARNING in ./node_modules/jsbi/dist/jsbi-umd.js
Module Warning (from ./node_modules/source-map-loader/dist/cjs.js):
Failed to parse source map from '/home/fs/code/git-repo/uniswapV3-prac/ui/node_modules/jsbi/lib/jsbi.ts' file: Error: ENOENT: no such file or directory, open '/home/fs/code/git-repo/uniswapV3-prac/ui/node_modules/jsbi/lib/jsbi.ts'
```

解决：在ui目录下，添加.env文件

```
GENERATE_SOURCEMAP = false
```



### 按钮不灵、WETH/USDC兑换不显示

```

```



### EIP-170 合约长度

```
== Logs ==
  WETH address 0x5FbDB2315678afecb367f032d93F642f64180aa3
  UNI address 0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0
  USDC address 0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512
  USDT address 0xDc64a140Aa3E981100a9becA4E685f962f0cF6C9
  WBTC address 0xCf7Ed3AccA5a467e9e704C703E8D87F634fB0Fc9
  Factory address 0x5FC8d32690cc91D4c39d9d3abcBD16989F875707
  Manager address 0x0165878A594ca255338adfa4d48449f69242Eb8F
  Quoter address 0xa513E6E4b8f2a923D98304ec87F64353C4D5C853
  USDT/USDC address 0x8cA66dCf12Ef6e2Db073b761D8DCEc2dE837a4E0
  WBTC/USDT address 0x3d006E4B23c6bBf9C55fD781c7c0cB7898929B05
  WETH/UNI address 0x370E7584F39505139D9B87b71B87D6C7A0ce3a93
  WETH/USDC address 0x0fbbBfe55079b8E4Ee9265e4866C3eBf78C0099B

`Unknown5` is above the EIP-170 contract size limit (25912 > 24576).
```
这个在dev4版本出现问题：



## 测试/精度

说明测试用例的数值的来源，以及可靠性。莫非是参考官方uniswapV3的测试文档？

还是说，用Python只能够大致校验这些值。

```bash
forge test -vv # 不同个数的v会增加相应的打印信息 最多vvvv
```

采用的是`assertEq()`，因此很多定点数的结果都需要所有小数位确定。

如果采用Python来计算：分别计算`price_to_tick()`和 `tick_to_price()`，得到的结果有一定的误差。

```python
print(MathTool.tick_to_price(85176))
print(MathTool.price_to_tick(4999.904785770063))

# 其中
def price_to_tick(p):
    return math.floor(math.log(p, 1.0001))
def tick_to_price(tick):
    return math.pow(1.0001, tick)

# 结果
4999.904785770063
85176
```

### 流动性:

### 流动性测试用例

流动性的测试用例包括 manager.t.sol、pool.t.sol，涉及到以下场景：

1. WETH/USDC交易对，现价5000，注入区间为[4545, 5500]，期望注入的数目为（1， 5000）。tickSpacing等于60。计算最终的流动性大小和两种交易对的具体数目。
2. 





#### python实现

```python
def mint(amount0, amount1, price_low, price_cur, price_upp):
    sqrtp_low = price_to_sqrtPriceX96(price_low)
    sqrtp_cur = price_to_sqrtPriceX96(price_cur)
    sqrtp_upp = price_to_sqrtPriceX96(price_upp)

    amount_0 = amount0 * eth
    amount_1 = amount1 * eth

    liq0 = liquidity0_X96(amount_0, sqrtp_cur, sqrtp_upp)
    liq1 = liquidity1_X96(amount_1, sqrtp_cur, sqrtp_low)
    liq = int(min(liq0, liq1))

    # if price_cur in tick range.
    amount_0_new = calc_amount0(liq, sqrtp_cur, sqrtp_upp) / eth
    amount_1_new = calc_amount1(liq, sqrtp_low, sqrtp_cur) / eth
    return (liq, amount_0_new, amount_1_new)
```

分别执行两个测试用例，可以看到其中值的不一致：

```python
# mints[0] = mintParams(4545, 5500, 1 ether, 5000 ether);
() = (0.987078348444137445, 5000) # solidity.
() = (0.9989766183474252, 5000.0) # python.

# mints[1] = mintParams(4000, 6250,(1 ether * 75) / 100, (5000 ether * 75) / 100);
() = (0.746110926570506489 ether, 5000 * 0.75 ether) # 根据solidity
() = (0.75, 3750.0) # in python.
```

对比`testMintOverlappingRanges()`，可以基本认为 `mint()`时，其他position不产生影响。

### 交易测试

#### 单区间交

```

```

#### 跨区间交易

```
```

### 交易费

交易费的计算过程主要是tick、position这两个概念。

当cur_price跨过当前tick的时候，会更新tick里面的值，同时判断是否需要

想要取出交易费：需要burn()对应的流动性，再调用collect()收取相应的token数量。



1. 创造position的时候
2. 发生交易的时候
3. 统计collect()的时候









