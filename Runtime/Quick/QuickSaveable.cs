namespace Unibrics.Saves.Quick
{
    using System;
    using API;

    public class QuickSaveable<T> : IQuickSaveable<T>, IQuickSaveable
    {
        public event Action ValueUpdated;

        public string Key { get; }

        public object GenericValue => value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                ValueUpdated?.Invoke();
            }
        }

        private T value;

        public QuickSaveable(string key)
        {
            Key = key;
        }

    }

    interface IQuickSaveable
    {
        string Key { get; }
        
        object GenericValue { get; }
    }
}