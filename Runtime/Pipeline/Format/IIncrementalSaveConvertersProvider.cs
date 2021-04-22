namespace Unibrics.Saves.Format
{
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Tools;

    interface IIncrementalSaveConvertersProvider
    {
        IEnumerable<IncrementalSaveFormatConverter> GetConverters();
    }

    class IncrementalSaveConvertersProvider : IIncrementalSaveConvertersProvider
    {
        public IEnumerable<IncrementalSaveFormatConverter> GetConverters()
        {
            return Types.AnnotatedWith<InstallAttribute>()
                .WithParent(typeof(IncrementalSaveFormatConverter))
                .TypesOnly()
                .CreateInstances<IncrementalSaveFormatConverter>();
        }
    }
}