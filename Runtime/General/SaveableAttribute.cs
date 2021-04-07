namespace Unibrics.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class SaveableAttribute : Attribute
    {
        public List<Type> BindTo { get; set; }

        public SaveableAttribute(params Type[] bindTo)
        {
            BindTo = bindTo.ToList();
        }
    }
}