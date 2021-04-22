namespace Unibrics.Saves.Format
{
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using Tools;

    public interface ISaveFormatConverter
    {
        JObject ConvertToLatest(JObject json);
    }

    class SaveFormatConverter : ISaveFormatConverter
    {
        private readonly int currentVersion;

        private readonly int minVersion;

        private readonly IncrementalSaveFormatConverter[] converters;

        public SaveFormatConverter(ISaveFormatVersionProvider versionProvider,
            IIncrementalSaveConvertersProvider provider)
        {
            currentVersion = versionProvider.SaveFormatVersion;
            minVersion = versionProvider.MinimumSupportedSaveFormatVersion;
            if (currentVersion < minVersion)
            {
                throw new SaveFormatConversionException(
                    $"Current save format version ({currentVersion}) " +
                    $"could not be less than minimal version ({minVersion})");
            }

            converters = provider.GetConverters().ToArray();
            
            for (var version = minVersion; version < currentVersion; version++)
            {
                EnsureSinglePathExistsFrom(version);
            }
            
            void EnsureSinglePathExistsFrom(int version)
            {
                var originalVersion = version;
                if (converters.Count(converter => converter.OriginalVersion == originalVersion) > 1)
                {
                    throw new SaveFormatConversionException(
                        $"More than one save converter for upgrading version {originalVersion} found");
                }

                foreach (var converter in converters)
                {
                    if (converter.OriginalVersion == version)
                    {
                        version = converter.GoalVersion;
                    }
                }

                if (version != currentVersion)
                {
                    throw new SaveFormatConversionException(
                        $"Can not find path for upgrading version {originalVersion} to {currentVersion}, only to {version} found");
                }
            }
        }

        public JObject ConvertToLatest(JObject json)
        {
            var version = json?["Header"]?["FormatVersion"]?.Value<int>();
            if (!version.HasValue)
            {
                throw new SaveFormatConversionException("Save is missing valid header");
            }

            if (version < minVersion)
            {
                //drop save with version less than minimal
                return new JObject();
            }

            if (version >= currentVersion)
            {
                return json;
            }

            foreach (var converter in converters)
            {
                if (converter.OriginalVersion == version.Value)
                {
                    converter.Process(json);
                }
            }

            return json;
        }
    }

    public class SaveFormatConversionException : UnibricsException
    {
        public SaveFormatConversionException(string message) : base(message)
        {
        }
    }
}