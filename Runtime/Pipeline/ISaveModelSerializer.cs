namespace Unibrics.Saves.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using Model;

    public interface ISaveModelSerializer
    {
        byte[] ConvertToBytes(SaveModel model);

        SaveParsingResult ConvertFromBytes(byte[] raw);
    }

    /// <summary>
    /// Multiple Pipelines serializer allows to convert from multiple pipelines and write with default one
    /// </summary>
    internal class MultiPipelineSaveModelSerializer : ISaveModelSerializer
    {
        private readonly ISavePipeline defaultPipeline;

        private readonly List<ISavePipeline> acceptablePipelines;

        public MultiPipelineSaveModelSerializer(ISavePipelinesProvider pipelinesProvider)
        {
            defaultPipeline = pipelinesProvider.GetDefaultPipeline();
            acceptablePipelines = pipelinesProvider.GetAcceptablePipelines();
        }

        public byte[] ConvertToBytes(SaveModel model)
        {
            return defaultPipeline.ConvertToBytes(model);
        }

        public SaveParsingResult ConvertFromBytes(byte[] raw)
        {
            if (raw == null || raw.Length == 0) 
            {
                return SaveParsingResult.Empty;
            }
            
            var headerOrError = SaveBinaryHeader.FromPrefixedArray(raw);
            if (!headerOrError.IsOk)
            {
                var headlessPipeline = acceptablePipelines.FirstOrDefault(pipeline => pipeline.Id == "headless");
                if (headlessPipeline == null)
                {
                    return SaveParsingResult.Corrupted;
                }

                return headlessPipeline.ConvertFromBytes(raw);
            }

            var header = headerOrError.Value;
            var payload = header.ExtractPayload(raw); 
            if (header.PipelineId == defaultPipeline.Id)
            {
                return defaultPipeline.ConvertFromBytes(payload);
            }

            var compatiblePipeline = acceptablePipelines.FirstOrDefault(pipeline => pipeline.Id == header.PipelineId);
            if (compatiblePipeline == null)
            {
                return SaveParsingResult.Corrupted;
            }

            return compatiblePipeline.ConvertFromBytes(payload);
        }
    }
}