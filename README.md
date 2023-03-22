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


本地的 foundry 版本有问题。