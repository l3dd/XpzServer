#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=xunit.runner.console&version=2.2.0"

var target          = Argument("target", "Default");
var configuration   = Argument("Configuration", "Release");
var artifactsDir    = Directory("./artifacts");
var solution        = "./src/Andoria.sln";
GitVersion versionInfo = null;

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./out/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("SetVersionInfo")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        versionInfo = GitVersion(new GitVersionSettings {
            RepositoryPath = "."
    });
});

Task("RestorePackages")
    .IsDependentOn("SetVersionInfo")
    .Does(() =>
{
    DotNetCoreRestore(solution);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("SetVersionInfo")
    .Does(() =>
{
    DotNetCoreRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var buildSettings = new DotNetCoreBuildSettings
     {
         Configuration = configuration,
         OutputDirectory = buildDir,
         ArgumentCustomization = args => args.Append("/p:SemVer=" + versionInfo.NuGetVersionV2)
     };

    // Use MSBuild
    DotNetCoreBuild("./src/Andoria.sln", buildSettings);
});
Task("Default")
  .Does(() =>
{
  Information("Hello World!");
});

RunTarget(target);
