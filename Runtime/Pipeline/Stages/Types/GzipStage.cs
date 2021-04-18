namespace Unibrics.Saves.Pipeline.Gzip
{
    using System.IO;
    using System.IO.Compression;
    
    [SavePipelineStage("compress.gzip")]
    public class GzipStage : SavePipelineStage<byte[], byte[]>
    {
        public override byte[] ProcessOut(byte[] data)
        {
            using var compressedStream = new MemoryStream();
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }

        public override byte[] ProcessIn(byte[] data)
        {
            using var compressedStream = new MemoryStream(data);
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }
}