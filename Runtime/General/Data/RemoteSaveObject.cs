namespace Unibrics.Saves
{
    using Model;

    class RemoteSaveObject : SaveObject
    {
        internal RemoteSaveObject(SaveParsingResult result) : base(result)
        {
        }

        public RemoteSaveObject(SaveModel model) : base(model)
        {
        }
    }
}