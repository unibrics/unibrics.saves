namespace Unibrics.Saves.API
{
    public interface ISaveContext
    {
        void Block(ISaveBlockingContext context);
        
        void Unblock(ISaveBlockingContext context);
    }
    
    public interface ISaveBlockingContext
    {
        public string Id { get; }
    }
}