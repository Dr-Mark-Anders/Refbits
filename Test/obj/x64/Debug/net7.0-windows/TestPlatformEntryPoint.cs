//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Microsoft.Testing.Platform.MSBuild
// </auto-generated>
//------------------------------------------------------------------------------

[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal sealed class TestingPlatformEntryPoint
{
    public static async global::System.Threading.Tasks.Task<int> Main(string[] args)
    {
        global::Microsoft.Testing.Platform.Builder.ITestApplicationBuilder builder = await global::Microsoft.Testing.Platform.Builder.TestApplication.CreateBuilderAsync(args);
        Microsoft.Testing.Platform.MSBuild.TestingPlatformBuilderHook.AddExtensions(builder, args);
        Microsoft.Testing.Extensions.Telemetry.TestingPlatformBuilderHook.AddExtensions(builder, args);
        using (global::Microsoft.Testing.Platform.Builder.ITestApplication app = await builder.BuildAsync())
        {
            return await app.RunAsync();
        }
    }
}