namespace Unibrics.Saves
{
    class InitialSaveObject : SaveObject
    {
        public override bool IsInitial => true;

        public InitialSaveObject() : base(null)
        {
        }
    }
}