namespace Unibrics.Saves.Pipeline
{
    using System;
    using JetBrains.Annotations;

    [MeansImplicitUse, AttributeUsage(AttributeTargets.Class)]
    public class SavePipelineStageAttribute : Attribute
    {
        public string Id { get; }

        public SavePipelineStageAttribute(string id)
        {
            Id = id;
        }
    }
}