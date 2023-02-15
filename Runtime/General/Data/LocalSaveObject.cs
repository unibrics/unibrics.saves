namespace Unibrics.Saves
{
    using Model;

    class LocalSaveObject : SaveObject
    {
        public override bool IsLocalSave => true;

        public LocalSaveObject(SaveModel model) : base(model) { }

        public LocalSaveObject(SaveParsingResult result) : base(result)
        {
        }
    }
}