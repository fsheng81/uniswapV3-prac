uniswapV3-prac to understand the framework of uniswapV3.

dev2：仅包括最基础的实现，单用户的mint()，不跨tick的swap()，不涉及solidity复杂的计算，也不涉及手续费的收取。以token0 = WETH, token1 = USDC为例，事先用Python实现计算功能，了解基本框架。

dev3：增加了工厂类、solidity的数学计算、多个池子计算、跨tick交易。不涉及交易费用、预言机、tickSpacing等。



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



UI相关设置：
设置app.js中的合约地址，并在浏览器中安装metamask插件
`npm install -g yarn`

执行 `cd ui & yarn & yarn start`
先执行yarn来检查packages
自动跳转 http://localhost:3000/
参考：https://github.com/facebook/create-react-app
App.js执行components的组件，组件最后执行content中的metaMask.js



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











