namespace UniswapV3Prac.Contracts.UniswapV3PoolTest.ContractDefinition

open System
open System.Threading.Tasks
open System.Collections.Generic
open System.Numerics
open Nethereum.Hex.HexTypes
open Nethereum.ABI.FunctionEncoding.Attributes

    type FuzzSelector() =
            [<Parameter("address", "addr", 1)>]
            member val public Addr = Unchecked.defaultof<string> with get, set
            [<Parameter("bytes4[]", "selectors", 2)>]
            member val public Selectors = Unchecked.defaultof<List<byte[]>> with get, set
    

