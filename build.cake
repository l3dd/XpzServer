#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=xunit.runner.console&version=2.2.0"

//////////////////////////////////////////////////////////////////////
// PARAMETERS
//////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var target          = Argument("target", "Default");
var configuration   = Argument("Configuration", "Release");


var OutputDirectory     = Directory("./out/");
var SrcDirectory        = Directory("./src/");
var BuildDirectory      = OutputDirectory + Directory("bin/");
var ArtifactDirectory   = OutputDirectory + Directory("Artifacts/");

var BuildLogFile        = ArtifactDirectory.Path + "/build.log";
var SolutionFile        = SrcDirectory.Path + "/Andoria.sln";
GitVersion versionInfo = null;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(OutputDirectory);
});

Task("SetVersionInfo")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        versionInfo = GitVersion(new GitVersionSettings {
            RepositoryPath = "."
    });

    Information("Calculated version is " + versionInfo.NuGetVersionV2 );
});

Task("RestorePackages")
    .IsDependentOn("SetVersionInfo")
    .Does(() =>
{
    DotNetCoreRestore(SolutionFile);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("SetVersionInfo")
    .Does(() =>
{
    DotNetCoreRestore(SolutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    var dotnetCoreBuildSettings = new DotNetCoreBuildSettings
     {
        Configuration = configuration,
        OutputDirectory = BuildDirectory,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
      } ;

    dotnetCoreBuildSettings.MSBuildSettings.TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error;
    dotnetCoreBuildSettings.MSBuildSettings.FileLoggers.Add(new MSBuildFileLoggerSettings { LogFile = BuildLogFile });
    dotnetCoreBuildSettings.MSBuildSettings.Properties.Add("SemVer", new List<string> { versionInfo.NuGetVersionV2 });

    // Use MSBuild
    DotNetCoreBuild(SolutionFile, dotnetCoreBuildSettings);
}).OnError(ex => {
    Error("Build Failed :(");
    throw ex;
}); 

Task("Default").IsDependentOn("Build");

RunTarget(target);
