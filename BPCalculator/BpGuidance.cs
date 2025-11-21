namespace BPCalculator
{
    public static class BpGuidance
    {
        public static string GuidanceFor(BPCategory cat) =>
            cat switch
            {
                BPCategory.High => "High BP: seek medical advice and consider lifestyle changes.",
                BPCategory.PreHigh => "Pre-High: recheck regularly; lifestyle changes recommended.",
                BPCategory.Ideal => "Ideal: maintain healthy lifestyle.",
                BPCategory.Low => "Low BP: check for symptoms; hydrate and consult if severe.",
                _ => "Unknown category."
            };
    }
}
