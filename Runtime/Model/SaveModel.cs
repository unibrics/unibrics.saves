namespace Unibrics.Saves.Model
{
    using System.Collections.Generic;
    using API;

    internal class SaveModel
    {
        public SerializationHeader Header { get; }
        
        public List<ISaveComponent> Components { get; private set; }

        public SaveModel(SerializationHeader header, List<ISaveComponent> components)
        {
            Header = header;
            Components = components;
        }
    }
}