namespace Unibrics.Saves.Pipeline
{
    using System;
    using System.Linq;
    using System.Text;
    using Exceptions;
    using Model;

    interface ISavePipeline
    {
        byte[] ConvertToBytes(SaveModel model);

        SaveModel ConvertFromBytes(byte[] raw);
    }

    class SavePipeline : ISavePipeline
    {
        private readonly ISavePipelineStage[] stages;

        private byte[] header;

        private string id;

        public SavePipeline(string id, ISavePipelineStage[] stages)
        {
            this.id = id;
            this.stages = stages;
            var idBytes = Encoding.UTF8.GetBytes(id);

            //header is keyword CA FE AB BA -id length (int) - id - payload
            header = new byte[] {0xCA, 0xFE, 0xAB, 0xBA}
                .Union(BitConverter.GetBytes(idBytes.Length))
                .Union(idBytes)
                .ToArray();

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

            var bytes = (byte[]) result;
        }

        public SaveModel ConvertFromBytes(byte[] raw)
        {
            object result = raw;

            //in stream bytes are processed in reversed order
            for (var i = stages.Length - 1; i >= 0; i--)
            {
                result = stages[i].ProcessInStream(result);
            }

            //type check is performed during initialization
            return (SaveModel) result;
        }
    }
}