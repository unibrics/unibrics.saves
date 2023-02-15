namespace Unibrics.Saves.Conflicts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SaveSummary<T> : ISaveSummary where T : SaveSummary<T>
    {
        public DateTime CreationTime { get; set; }
        
        public string DeviceFingerprint { get; set; }

        public int Version { get; set; }

        public SaveCompareResult CompareTo(ISaveSummary summary)
        {
            if (!(summary is T typed))
            {
                throw new ArgumentException($"{typeof(SaveSummary<T>)} expected, but get {summary.GetType()}");
            }

            //if both saves came from the same device, we choose newer one
            if (DeviceFingerprint == summary.DeviceFingerprint)
            {
                return CreationTime.CompareTo(summary.CreationTime).ToSaveCompareResult();
            }

            //otherwise we're looking into it
            var res = GetComparisonResults(typed).Distinct().ToList();
            if (res.Contains(SaveCompareResult.Unknown))
            {
                return SaveCompareResult.Unknown;
            }

            if (res.Contains(SaveCompareResult.Greater) &&
                res.Contains(SaveCompareResult.Less))
            {
                return SaveCompareResult.Unknown;
            }

            if (res.Contains(SaveCompareResult.Greater))
            {
                return SaveCompareResult.Greater;
            }

            if (res.Contains(SaveCompareResult.Less))
            {
                return SaveCompareResult.Less;
            }

            return SaveCompareResult.Equal;
        }

        private IEnumerable<SaveCompareResult> GetComparisonResults(T save)
        {
            yield return Version.CompareTo(save.Version).ToSaveCompareResult();
            yield return CreationTime.CompareTo(save.CreationTime).ToSaveCompareResult();
            foreach (var compareResult in GetAdditionalComparisonResults(save))
            {
                yield return compareResult;
            }
        }

        protected virtual IEnumerable<SaveCompareResult> GetAdditionalComparisonResults(T save)
        {
            yield break;
        }
    }
}