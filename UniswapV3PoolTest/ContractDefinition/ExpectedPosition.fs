namespace UniswapV3Prac.Contracts.UniswapV3PoolTest.ContractDefinition

open System
open System.Threading.Tasks
open System.Collections.Generic
open System.Numerics
open Nethereum.Hex.HexTypes
open Nethereum.ABI.FunctionEncoding.Attributes

    type ExpectedPosition() =
            [<Parameter("address", "pool", 1)>]
            member val public Pool = Unchecked.defaultof<string> with get, set
            [<Parameter("address", "owner", 2)>]
            member val public Owner = Unchecked.defaultof<string> with get, set
            [<Parameter("int24[2]", "ticks", 3)>]
            member val public Ticks = Unchecked.defaultof<List<int>> with get, set
            [<Parameter("uint128", "liquidity", 4)>]
            member val public Liquidity = Unchecked.defaultof<BigInteger> with get, set
            [<Parameter("uint256[2]", "feeGrowth", 5)>]
            member val public FeeGrowth = Unchecked.defaultof<List<BigInteger>> with get, set
            [<Parameter("uint128[2]", "tokensOwed", 6)>]
            member val public TokensOwed = Unchecked.defaultof<List<BigInteger>> with get, set
    

