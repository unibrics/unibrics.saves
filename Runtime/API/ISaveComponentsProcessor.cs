namespace Unibrics.Saves.API
{
    using Core;

    /// <summary>
    /// Use this interface in case you need to adjust/alter/replace savecomponent after its deserialization or initial
    /// creation and before calling Deserialize() method with it
    /// </summary>
    public interface ISaveComponentsProcessor
    {
        Priority Priority { get; }

        void ProcessExistingComponent(ISaveComponent saveComponent);
        
        void ProcessNewComponent(ISaveComponent saveComponent);
    }
}