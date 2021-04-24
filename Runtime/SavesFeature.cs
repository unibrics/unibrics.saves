namespace Unibrics.Saves
{
    using Core.Features;

    [AppFeature]
    public class SavesFeature : AppFeature
    {
        public override string Id => "feature.saves";

        public override bool SupportsSuspension => true;
    }
}