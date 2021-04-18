namespace Unibrics.Saves.Pipeline
{
    using System.Linq;
    using Exceptions;
    using Model;

    interface ISavePipeline : ISaveModelSerializer
    {
        public string Id { get; }
    }

    class SavePipeline : ISavePipeline
    {
        public string Id { get; }
        
        private readonly ISavePipelineStage[] stages;

        private readonly SaveBinaryHeader header;

        public SavePipeline(string id, ISavePipelineStage[] stages)
        {
            Id = id;
            this.stages = stages;

            header = SaveBinaryHeader.FromPipelineId(id);

            if (stages.Length < 1)
            {
                throw new BrokenSavePipelineException("Can not create pipeline with no stages");
            }

            if (!(stages.First() is ISavePipelineInboundStage<SaveModel>))
            {
                throw new BrokenSavePipelineException($"First stage must take {nameof(SaveModel)} as parameter");
            }

            if (!(stages.Last() is ISavePipelineOutboundStage<byte[]>))
            {
                throw new BrokenSavePipelineException($"Last stage must produce byte[] as parameter");
            }

            for (var i = 1; i < stages.Length; i++)
            {
                var stage = stages[i];
                var previousStage = stages[i - 1];
                if (!stage.IsBindableTo(previousStage))
                {
                    throw new BrokenSavePipelineException($"Stage {stage} can not be bind to {previousStage}");
                }
            }
        }

        

        public byte[] ConvertToBytes(SaveModel model)
        {
            object result = model;
            for (var i = 0; i < stages.Length; i++)
            {
                result = stages[i].ProcessOutStream(result);
            }

            return header.AddItselfTo((byte[]) result);
        }

        public SaveParsingResult ConvertFromBytes(byte[] raw)
        {
            object result = raw;

            //in stream bytes are processed in reversed order
            for (var i = stages.Length - 1; i >= 0; i--)
            {
                result = stages[i].ProcessInStream(result);
            }

            //type check is performed during initialization
            return new SaveParsingResult((SaveModel) result);
        }
    }
}