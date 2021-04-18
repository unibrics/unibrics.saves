namespace Unibrics.Saves.Pipeline
{
    public abstract class SavePipelineStage<TIn, TOut> : ISavePipelineStage<TIn, TOut>
    {
        public abstract TOut ProcessOut(TIn model);
        
        public abstract TIn ProcessIn(TOut data);

        public bool IsBindableTo(ISavePipelineStage stage)
        {
            return stage is ISavePipelineOutboundStage<TIn>;
        }

        public object ProcessOutStream(object data)
        {
            return ProcessOut((TIn) data);
        }
        
        public object ProcessInStream(object data)
        {
            return ProcessIn((TOut) data);
        }
    }
}