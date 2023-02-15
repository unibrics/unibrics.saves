namespace Unibrics.Saves.Conflicts
{
    public static class SaveSummaryExtensions
    {
        public static SaveCompareResult ToSaveCompareResult(this int compare)
        {
            switch (compare)
            {
                case 1: return SaveCompareResult.Greater;
                case 0: return SaveCompareResult.Equal;
                case -1: return SaveCompareResult.Less;
            }

            return SaveCompareResult.Unknown;
        }
    }
}