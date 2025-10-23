namespace F23.StringSimilarity.Benchmarks;

public record BuildConfiguration(string? Configuration = null, string? PackageVersion = null)
{
    public string Id => Configuration ?? PackageVersion ?? throw new InvalidOperationException("Either Configuration or PackageVersion must be set.");

    public static IEnumerable<BuildConfiguration> BuildConfigurations
    {
        get
        {
            // Local build
            yield return new BuildConfiguration(Configuration: "LocalBuild");

            // Different package versions
            //yield return new BuildConfiguration(PackageVersion: "6.0.0");
            yield return new BuildConfiguration(PackageVersion: "7.0.0");
        }
    }
}
