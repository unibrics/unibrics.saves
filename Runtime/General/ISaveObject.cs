namespace Unibrics.Saves
{
    using Model;

    interface ISaveObject
    {
        SaveModel Result { get; }
        
        bool IsInitial { get; }
        
        bool IsEmpty { get; }
    }

    abstract class SaveObject : ISaveObject
    {
        public virtual bool IsLocalSave { get; }

        public virtual bool IsInitial { get; }

        public SaveModel Result { get; set; }

        public bool IsEmpty => Result == null || Result.Components.Count == 0;
        
        public bool HasErrors { get; private set; }

        //public bool SaveCameFromNewerBuild => Result != null && Result.CameFromNewBuild;
        
        protected SaveObject(SaveModel model)
        {
            Result = model;
        }
    }

    class LocalSaveObject : SaveObject
    {
        public override bool IsLocalSave => true;

        public LocalSaveObject(SaveModel model) : base(model) { }
    }
    
    class InitialSaveObject : SaveObject
    {
        public override bool IsInitial => true;

        public InitialSaveObject() : base(null)
        {
        }
    }
}