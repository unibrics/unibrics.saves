namespace Unibrics.Saves.Pipeline
{
    public interface ISavePipelineStage
    {
        bool IsBindableTo(ISavePipelineStage stage);

        /// <summary>
        /// [SaveModel] -> ProcessOutStream -> [byte[]]
        /// </summary>
        object ProcessOutStream(object data);
        
        /// <summary>
        /// [byte[]] -> ProcessInStream -> [SaveModel]
        /// </summary>
        object ProcessInStream(object data);
    }

    public interface ISavePipelineInboundStage<TIn> : ISavePipelineStage
    {
    }

    public interface ISavePipelineOutboundStage<TOut> : ISavePipelineStage
    {
    }

    public interface ISavePipelineStage<TIn, TOut> : ISavePipelineInboundStage<TIn>, ISavePipelineOutboundStage<TOut>
    {
        TOut ProcessOut(TIn model);
        
        TIn ProcessIn(TOut data);
    }
}