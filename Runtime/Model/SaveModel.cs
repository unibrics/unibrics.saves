namespace Unibrics.Saves.Model
{
    using System.Collections.Generic;
    using API;

    public class SaveModel
    {
        public SerializationHeader Header { get; }
        
        public string Group { get; }
        
        public List<ISaveComponent> Components { get; private set; }

        public SaveModel(SerializationHeader header, List<ISaveComponent> components)
        {
            Header = header;
            Group = header.GroupName;
            Components = components;
        }
    }
}