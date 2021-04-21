namespace Unibrics.Saves.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Core.DI;
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

        private readonly IInstanceProvider instanceProvider;

        public SavePipelineFactory(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
            
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

                stages[i] = (ISavePipelineStage)instanceProvider.GetInstance(availableStageTypes[id]);
            }

            return new SavePipeline(settings.Id, stages);
        }
    }
}