namespace Unibrics.Saves.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Exceptions;
    using Tools;

    interface ISavePipelineFactory
    {
        ISavePipeline CreatePipeline(List<string> stageIds);
    }

    class SavePipelineFactory : ISavePipelineFactory
    {
        private readonly IDictionary<string, Type> availableStageTypes;

        public SavePipelineFactory()
        {
            availableStageTypes = Types.AnnotatedWith<SavePipelineStageAttribute>()
                .WithParent(typeof(ISavePipelineStage))
                .ToDictionary(tuple => tuple.attribute.Id, tuple => tuple.type);
        }

        public ISavePipeline CreatePipeline(List<string> stageIds)
        {
            var stages = new ISavePipelineStage[stageIds.Count];
            for (var i = 0; i < stageIds.Count; i++)
            {
                var id = stageIds[i];
                if (!availableStageTypes.ContainsKey(id))
                {
                    throw new BrokenSavePipelineException($"Unknown stage id {id}");
                }

                stages[i] = (ISavePipelineStage) Activator.CreateInstance(availableStageTypes[id]);
            }

            return new SavePipeline(stages);
        }
    }
}