namespace Unibrics.Saves.API
{
    public interface IFirstSessionChecker
    {
        bool IsFirstSession { get; }

        internal void MarkCurrentSessionAsFirst();
    }

    class SimpleFirstSessionChecker : IFirstSessionChecker
    {
        public bool IsFirstSession { get; private set; }
        void IFirstSessionChecker.MarkCurrentSessionAsFirst()
        {
            IsFirstSession = true;
        }
    }
}