namespace Unibrics.Saves.Pipeline
{
    public class SaveBinaryHeader
    {
        public string PipelineId { get; }
        
        private byte[] binary;

        public SaveBinaryHeader(byte[] binary)
        {
            this.binary = binary;
        }

        public static SaveBinaryHeader FromPipelineId(string id)
        {
            
        }
        
        public static SaveBinaryHeader ExtractFromSave
    }
}