#addin nuget:?package=Cake.FileHelpers&version=1.0.4
#addin nuget:?package=Cake.Xamarin&version=1.3.0.15
#tool nuget:?package=vswhere
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var version = EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? Argument("version", "0.0.9999");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var vsLatest = VSWhereLatest();
var msBuildPath = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");
var solution = "./Iconize.sln";
var nuspec = new List<FilePath> {
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.nuspec") },
	{ new FilePath("./NuGet/Xam.FormsPlugin.Iconize.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.EntypoPlus.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.FontAwesome.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.Ionicons.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.Material.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.Meteocons.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.SimpleLineIcons.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.Typicons.nuspec") },
	{ new FilePath("./NuGet/Xam.Plugin.Iconize.WeatherIcons.nuspec") }
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
		// Use MSBuild
		MSBuild(solution, settings =>{
			settings.SetConfiguration(configuration);
			settings.ToolPath = msBuildPath; 	
		});
    }
    else
    {
		// Use XBuild
		XBuild(solution, settings =>
			settings.SetConfiguration(configuration));
    }
});

Task("NuGet")
	.IsDependentOn("Build")
	.Does (() =>
{
    if(!DirectoryExists("./build/nuget/"))
        CreateDirectory("./build/nuget");
        
	NuGetPack(nuspec, new NuGetPackSettings { 
		Version = version,
		Verbosity = NuGetVerbosity.Detailed,
		OutputDirectory = "./build/nuget/",
		BasePath = "./",
	});	
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
	.IsDependentOn("NuGet");

Task("Clean")
	.Does(() =>
{
	CleanDirectory("./tools/");
	CleanDirectories("./build/");

	CleanDirectories("./**/bin");
	CleanDirectories("./**/obj");
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);