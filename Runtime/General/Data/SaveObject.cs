namespace Unibrics.Saves
{
    using Model;

    public abstract class SaveObject : ISaveObject
    {
        public virtual bool IsLocalSave { get; }

        public virtual bool IsInitial { get; }

        public SaveModel Result { get; set; }
        
        public int BytesTotal { get; }

        public bool IsEmpty => Result == null || Result.Components.Count == 0;
        
        public bool HasErrors { get; private set; }

        protected SaveObject(SaveModel model)
        {
            Result = model;
        }
        
        internal SaveObject(SaveParsingResult result)
        {
            Result = result.SaveModel;
            HasErrors = result.HasErrors;
            BytesTotal = result.BytesTotal;
        }
    }
}