namespace Unibrics.Saves.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Exceptions;
    using Settings;
    using Tools;

    interface ISavePipelineFactory
    {
        ISavePipeline CreatePipeline(SavePipelineSettings settings);
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

        public ISavePipeline CreatePipeline(SavePipelineSettings settings)
        {
            var stageIds = settings.Stages;
            var stages = new ISavePipelineStage[stageIds.Length];
            for (var i = 0; i < stageIds.Length; i++)
            {
                var id = stageIds[i];
                if (!availableStageTypes.ContainsKey(id))
                {
                    throw new BrokenSavePipelineException($"Unknown stage id {id}");
                }

                stages[i] = (ISavePipelineStage) Activator.CreateInstance(availableStageTypes[id]);
            }

            return new SavePipeline(settings.Id, stages);
        }
    }
}