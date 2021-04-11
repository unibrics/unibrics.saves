namespace Unibrics.Saves.Exceptions
{
    using Tools;

    public class BrokenSavePipelineException : UnibricsException
    {
        public BrokenSavePipelineException(string message) : base(message)
        {
        }
    }
}