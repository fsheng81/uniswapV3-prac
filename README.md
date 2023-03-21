# uniswapV3-prac to understand the framework of uniswapV3

``` bash
forge install foundry-rs/forge-std
forge install Rari-Capital/solmate
anvil # 另一个bash
forge test
```
结果:
Encountered a total of 6 failing tests, 8 tests succeeded


为了解决vscode的报错，新增remappings.txt

forge build

forge create <filePath>:<ContractName> --private-key=$PRIV_KEY

部署脚本：
forge script scripts/DeployDevelopment.s.sol --broadcast --fork-url http://localhost:8545 --private-key $PRIVATE_KEY

forge script scripts/DeployDevelopment.s.sol --broadcast --fork-url http://localhost:8545 --private-key 0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266
