namespace Unibrics.Saves.General
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class SaveableAttribute : Attribute
    {
        public Type[] BindTo { get; set; }

        public SaveableAttribute(params Type[] bindTo)
        {
            BindTo = bindTo;
        }
    }
}