namespace Unibrics.Saves.Pipeline
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Utils;

    public class SaveBinaryHeader
    {
        public string PipelineId { get; }

        private readonly byte[] header;

        /// <summary>
        /// Magic bytes are 4 specific bytes at the start of a file we're using to
        /// determine that save has correct header
        /// </summary>
        private static readonly byte[] MagicBytes = {0xCA, 0xFE, 0xAB, 0xBA};

        private SaveBinaryHeader(string id)
        {
            PipelineId = id;

            var idBytes = Encoding.UTF8.GetBytes(id);

            //header is keyword CA FE AB BA -id length (int) - id - payload
            header = MagicBytes
                .Concat(BitConverter.GetBytes(idBytes.Length))
                .Concat(idBytes)
                .ToArray();
        }

        internal static SaveBinaryHeader FromPipelineId(string id)
        {
            return new SaveBinaryHeader(id);
        }

        internal static ValueOrError<SaveBinaryHeader, SaveHeaderError> FromPrefixedArray(byte[] array)
        {
            if (array == null || array.Length < 8)
            {
                return ValueOrError<SaveBinaryHeader, SaveHeaderError>.FromError(SaveHeaderError.ArrayIsToShort);
            }

            for (var i = 0; i < 4; i++)
            {
                if (array[i] != MagicBytes[i])
                {
                    return ValueOrError<SaveBinaryHeader, SaveHeaderError>.FromError(SaveHeaderError.WrongHeader);
                }
            }

            var length = BitConverter.ToInt32(array, 4);
            var id = Encoding.UTF8.GetString(array, 8, length);
            return new SaveBinaryHeader(id);
        }

        public byte[] AddItselfTo(byte[] payload)
        {
            var result = new byte[payload.Length + header.Length];
            Array.Copy(header, 0, result, 0, header.Length);
            Array.Copy(payload, 0, result, header.Length, payload.Length);
            return result;
        }

        public byte[] ExtractPayload(byte[] data)
        {
            var result = new byte[data.Length - header.Length];
            Array.Copy(data, header.Length, result, 0, result.Length);
            return result;
        }

        internal enum SaveHeaderError
        {
            ArrayIsToShort,
            WrongHeader
        }
    }
}