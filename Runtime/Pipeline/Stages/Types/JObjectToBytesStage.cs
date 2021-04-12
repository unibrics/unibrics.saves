namespace Unibrics.Saves.Pipeline
{
    using System.Text;
    using Newtonsoft.Json.Linq;

    [SavePipelineStage("convert.jobject.bytes")]
    class JObjectToBytesStage : SavePipelineStage<JObject, byte[]>
    {
        public override byte[] ProcessOut(JObject model)
        {
            return Encoding.UTF8.GetBytes(model.ToString());
        }

        public override JObject ProcessIn(byte[] data)
        {
            return JObject.Parse(Encoding.UTF8.GetString(data));
        }
    }
}