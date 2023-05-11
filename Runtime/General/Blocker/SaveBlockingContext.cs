namespace Unibrics.Saves.Blocker
{
    using API;

    public struct SaveBlockingContext : ISaveBlockingContext
    {
        public string Id { get; set; }

        public static ISaveBlockingContext Initial => WithId("initial");
        
        public static ISaveBlockingContext WithId(string id) => new SaveBlockingContext() { Id = id };
    }
}