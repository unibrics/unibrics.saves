namespace Unibrics.Saves.Pipeline
{
    using System;
    using System.Linq;
    using System.Text;

    public class SaveBinaryHeader
    {
        public string PipelineId { get; }
        
        private byte[] binary;

        private SaveBinaryHeader(string id)
        {
            PipelineId = id;

            var idBytes = Encoding.UTF8.GetBytes(id);
            
            //header is keyword CA FE AB BA -id length (int) - id - payload
            binary = new byte[] {0xCA, 0xFE, 0xAB, 0xBA}
                .Union(BitConverter.GetBytes(idBytes.Length))
                .Union(idBytes)
                .ToArray();
        }

        public static SaveBinaryHeader FromPipelineId(string id)
        {
            return new SaveBinaryHeader(id);
        }

        public byte[] AddItselfTo(byte[] data)
        {
            var result = new byte[data.Length + binary.Length];
            Array.Copy(binary, 0, result, 0, binary.Length);
            Array.Copy(data, 0, result, binary.Length, data.Length);
            return result;
        }
    }
}