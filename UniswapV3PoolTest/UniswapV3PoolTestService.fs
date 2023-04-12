namespace UniswapV3Prac.Contracts.UniswapV3PoolTest

open System
open System.Threading.Tasks
open System.Collections.Generic
open System.Numerics
open Nethereum.Hex.HexTypes
open Nethereum.ABI.FunctionEncoding.Attributes
open Nethereum.Web3
open Nethereum.RPC.Eth.DTOs
open Nethereum.Contracts.CQS
open Nethereum.Contracts.ContractHandlers
open Nethereum.Contracts
open System.Threading
open UniswapV3Prac.Contracts.UniswapV3PoolTest.ContractDefinition


    type UniswapV3PoolTestService (web3: Web3, contractAddress: string) =
    
        member val Web3 = web3 with get
        member val ContractHandler = web3.Eth.GetContractHandler(contractAddress) with get
    
        static member DeployContractAndWaitForReceiptAsync(web3: Web3, uniswapV3PoolTestDeployment: UniswapV3PoolTestDeployment, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> = 
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            web3.Eth.GetContractDeploymentHandler<UniswapV3PoolTestDeployment>().SendRequestAndWaitForReceiptAsync(uniswapV3PoolTestDeployment, cancellationTokenSourceVal)
        
        static member DeployContractAsync(web3: Web3, uniswapV3PoolTestDeployment: UniswapV3PoolTestDeployment): Task<string> =
            web3.Eth.GetContractDeploymentHandler<UniswapV3PoolTestDeployment>().SendRequestAsync(uniswapV3PoolTestDeployment)
        
        static member DeployContractAndGetServiceAsync(web3: Web3, uniswapV3PoolTestDeployment: UniswapV3PoolTestDeployment, ?cancellationTokenSource : CancellationTokenSource) = async {
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            let! receipt = UniswapV3PoolTestService.DeployContractAndWaitForReceiptAsync(web3, uniswapV3PoolTestDeployment, cancellationTokenSourceVal) |> Async.AwaitTask
            return new UniswapV3PoolTestService(web3, receipt.ContractAddress);
            }
    
        member this.IS_TESTQueryAsync(iS_TESTFunction: IS_TESTFunction, ?blockParameter: BlockParameter): Task<bool> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<IS_TESTFunction, bool>(iS_TESTFunction, blockParameterVal)
            
        member this.AssertPositionRequestAsync(assertPositionFunction: AssertPositionFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(assertPositionFunction);
        
        member this.AssertPositionRequestAndWaitForReceiptAsync(assertPositionFunction: AssertPositionFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(assertPositionFunction, cancellationTokenSourceVal);
        
        member this.ExcludeArtifactsQueryAsync(excludeArtifactsFunction: ExcludeArtifactsFunction, ?blockParameter: BlockParameter): Task<List<string>> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<ExcludeArtifactsFunction, List<string>>(excludeArtifactsFunction, blockParameterVal)
            
        member this.ExcludeContractsQueryAsync(excludeContractsFunction: ExcludeContractsFunction, ?blockParameter: BlockParameter): Task<List<string>> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<ExcludeContractsFunction, List<string>>(excludeContractsFunction, blockParameterVal)
            
        member this.ExcludeSendersQueryAsync(excludeSendersFunction: ExcludeSendersFunction, ?blockParameter: BlockParameter): Task<List<string>> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<ExcludeSendersFunction, List<string>>(excludeSendersFunction, blockParameterVal)
            
        member this.FailedRequestAsync(failedFunction: FailedFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(failedFunction);
        
        member this.FailedRequestAndWaitForReceiptAsync(failedFunction: FailedFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(failedFunction, cancellationTokenSourceVal);
        
        member this.SetUpRequestAsync(setUpFunction: SetUpFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(setUpFunction);
        
        member this.SetUpRequestAndWaitForReceiptAsync(setUpFunction: SetUpFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(setUpFunction, cancellationTokenSourceVal);
        
        member this.TargetArtifactSelectorsQueryAsync(targetArtifactSelectorsFunction: TargetArtifactSelectorsFunction, ?blockParameter: BlockParameter): Task<TargetArtifactSelectorsOutputDTO> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryDeserializingToObjectAsync<TargetArtifactSelectorsFunction, TargetArtifactSelectorsOutputDTO>(targetArtifactSelectorsFunction, blockParameterVal)
            
        member this.TargetArtifactsQueryAsync(targetArtifactsFunction: TargetArtifactsFunction, ?blockParameter: BlockParameter): Task<List<string>> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TargetArtifactsFunction, List<string>>(targetArtifactsFunction, blockParameterVal)
            
        member this.TargetContractsQueryAsync(targetContractsFunction: TargetContractsFunction, ?blockParameter: BlockParameter): Task<List<string>> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TargetContractsFunction, List<string>>(targetContractsFunction, blockParameterVal)
            
        member this.TargetSelectorsQueryAsync(targetSelectorsFunction: TargetSelectorsFunction, ?blockParameter: BlockParameter): Task<TargetSelectorsOutputDTO> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryDeserializingToObjectAsync<TargetSelectorsFunction, TargetSelectorsOutputDTO>(targetSelectorsFunction, blockParameterVal)
            
        member this.TargetSendersQueryAsync(targetSendersFunction: TargetSendersFunction, ?blockParameter: BlockParameter): Task<List<string>> =
            let blockParameterVal = defaultArg blockParameter null
            this.ContractHandler.QueryAsync<TargetSendersFunction, List<string>>(targetSendersFunction, blockParameterVal)
            
        member this.TestBurnRequestAsync(testBurnFunction: TestBurnFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testBurnFunction);
        
        member this.TestBurnRequestAndWaitForReceiptAsync(testBurnFunction: TestBurnFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testBurnFunction, cancellationTokenSourceVal);
        
        member this.TestBurnPartiallyRequestAsync(testBurnPartiallyFunction: TestBurnPartiallyFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testBurnPartiallyFunction);
        
        member this.TestBurnPartiallyRequestAndWaitForReceiptAsync(testBurnPartiallyFunction: TestBurnPartiallyFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testBurnPartiallyFunction, cancellationTokenSourceVal);
        
        member this.TestCollectRequestAsync(testCollectFunction: TestCollectFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testCollectFunction);
        
        member this.TestCollectRequestAndWaitForReceiptAsync(testCollectFunction: TestCollectFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testCollectFunction, cancellationTokenSourceVal);
        
        member this.TestCollectAfterZeroBurnRequestAsync(testCollectAfterZeroBurnFunction: TestCollectAfterZeroBurnFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testCollectAfterZeroBurnFunction);
        
        member this.TestCollectAfterZeroBurnRequestAndWaitForReceiptAsync(testCollectAfterZeroBurnFunction: TestCollectAfterZeroBurnFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testCollectAfterZeroBurnFunction, cancellationTokenSourceVal);
        
        member this.TestCollectMoreThanAvailableRequestAsync(testCollectMoreThanAvailableFunction: TestCollectMoreThanAvailableFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testCollectMoreThanAvailableFunction);
        
        member this.TestCollectMoreThanAvailableRequestAndWaitForReceiptAsync(testCollectMoreThanAvailableFunction: TestCollectMoreThanAvailableFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testCollectMoreThanAvailableFunction, cancellationTokenSourceVal);
        
        member this.TestCollectPartiallyRequestAsync(testCollectPartiallyFunction: TestCollectPartiallyFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testCollectPartiallyFunction);
        
        member this.TestCollectPartiallyRequestAndWaitForReceiptAsync(testCollectPartiallyFunction: TestCollectPartiallyFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testCollectPartiallyFunction, cancellationTokenSourceVal);
        
        member this.TestFlashRequestAsync(testFlashFunction: TestFlashFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testFlashFunction);
        
        member this.TestFlashRequestAndWaitForReceiptAsync(testFlashFunction: TestFlashFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testFlashFunction, cancellationTokenSourceVal);
        
        member this.TestInitializeRequestAsync(testInitializeFunction: TestInitializeFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testInitializeFunction);
        
        member this.TestInitializeRequestAndWaitForReceiptAsync(testInitializeFunction: TestInitializeFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testInitializeFunction, cancellationTokenSourceVal);
        
        member this.TestMintInRangeRequestAsync(testMintInRangeFunction: TestMintInRangeFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintInRangeFunction);
        
        member this.TestMintInRangeRequestAndWaitForReceiptAsync(testMintInRangeFunction: TestMintInRangeFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintInRangeFunction, cancellationTokenSourceVal);
        
        member this.TestMintInsufficientTokenBalanceRequestAsync(testMintInsufficientTokenBalanceFunction: TestMintInsufficientTokenBalanceFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintInsufficientTokenBalanceFunction);
        
        member this.TestMintInsufficientTokenBalanceRequestAndWaitForReceiptAsync(testMintInsufficientTokenBalanceFunction: TestMintInsufficientTokenBalanceFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintInsufficientTokenBalanceFunction, cancellationTokenSourceVal);
        
        member this.TestMintInvalidTickRangeLowerRequestAsync(testMintInvalidTickRangeLowerFunction: TestMintInvalidTickRangeLowerFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintInvalidTickRangeLowerFunction);
        
        member this.TestMintInvalidTickRangeLowerRequestAndWaitForReceiptAsync(testMintInvalidTickRangeLowerFunction: TestMintInvalidTickRangeLowerFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintInvalidTickRangeLowerFunction, cancellationTokenSourceVal);
        
        member this.TestMintInvalidTickRangeUpperRequestAsync(testMintInvalidTickRangeUpperFunction: TestMintInvalidTickRangeUpperFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintInvalidTickRangeUpperFunction);
        
        member this.TestMintInvalidTickRangeUpperRequestAndWaitForReceiptAsync(testMintInvalidTickRangeUpperFunction: TestMintInvalidTickRangeUpperFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintInvalidTickRangeUpperFunction, cancellationTokenSourceVal);
        
        member this.TestMintOverlappingRangesRequestAsync(testMintOverlappingRangesFunction: TestMintOverlappingRangesFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintOverlappingRangesFunction);
        
        member this.TestMintOverlappingRangesRequestAndWaitForReceiptAsync(testMintOverlappingRangesFunction: TestMintOverlappingRangesFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintOverlappingRangesFunction, cancellationTokenSourceVal);
        
        member this.TestMintRangeAboveRequestAsync(testMintRangeAboveFunction: TestMintRangeAboveFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintRangeAboveFunction);
        
        member this.TestMintRangeAboveRequestAndWaitForReceiptAsync(testMintRangeAboveFunction: TestMintRangeAboveFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintRangeAboveFunction, cancellationTokenSourceVal);
        
        member this.TestMintRangeBelowRequestAsync(testMintRangeBelowFunction: TestMintRangeBelowFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintRangeBelowFunction);
        
        member this.TestMintRangeBelowRequestAndWaitForReceiptAsync(testMintRangeBelowFunction: TestMintRangeBelowFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintRangeBelowFunction, cancellationTokenSourceVal);
        
        member this.TestMintZeroLiquidityRequestAsync(testMintZeroLiquidityFunction: TestMintZeroLiquidityFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(testMintZeroLiquidityFunction);
        
        member this.TestMintZeroLiquidityRequestAndWaitForReceiptAsync(testMintZeroLiquidityFunction: TestMintZeroLiquidityFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(testMintZeroLiquidityFunction, cancellationTokenSourceVal);
        
        member this.UniswapV3FlashCallbackRequestAsync(uniswapV3FlashCallbackFunction: UniswapV3FlashCallbackFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(uniswapV3FlashCallbackFunction);
        
        member this.UniswapV3FlashCallbackRequestAndWaitForReceiptAsync(uniswapV3FlashCallbackFunction: UniswapV3FlashCallbackFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(uniswapV3FlashCallbackFunction, cancellationTokenSourceVal);
        
        member this.UniswapV3MintCallbackRequestAsync(uniswapV3MintCallbackFunction: UniswapV3MintCallbackFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(uniswapV3MintCallbackFunction);
        
        member this.UniswapV3MintCallbackRequestAndWaitForReceiptAsync(uniswapV3MintCallbackFunction: UniswapV3MintCallbackFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(uniswapV3MintCallbackFunction, cancellationTokenSourceVal);
        
        member this.UniswapV3SwapCallbackRequestAsync(uniswapV3SwapCallbackFunction: UniswapV3SwapCallbackFunction): Task<string> =
            this.ContractHandler.SendRequestAsync(uniswapV3SwapCallbackFunction);
        
        member this.UniswapV3SwapCallbackRequestAndWaitForReceiptAsync(uniswapV3SwapCallbackFunction: UniswapV3SwapCallbackFunction, ?cancellationTokenSource : CancellationTokenSource): Task<TransactionReceipt> =
            let cancellationTokenSourceVal = defaultArg cancellationTokenSource null
            this.ContractHandler.SendRequestAndWaitForReceiptAsync(uniswapV3SwapCallbackFunction, cancellationTokenSourceVal);
        
    

