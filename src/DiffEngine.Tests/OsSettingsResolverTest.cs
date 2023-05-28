public class OsSettingsResolverTest :
    XunitContextBase
{
    [Fact]
    public void Simple()
    {
        var paths = OsSettingsResolver.ExpandProgramFiles(new[] { "Path" }).ToList();
        Assert.Equal("Path", paths.Single());
    }

    [Fact]
    public void Expand()
    {
        var paths = OsSettingsResolver.ExpandProgramFiles(new[] { @"%ProgramFiles%\Path" }).ToList();
        Assert.Equal(@"%ProgramFiles%\Path", paths[0]);
        Assert.Equal(@"%ProgramW6432%\Path", paths[1]);
        Assert.Equal(@"%ProgramFiles(x86)%\Path", paths[2]);
    }

    [Fact]
    public void EnvPath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var found = OsSettingsResolver.TryFindInEnvPath("cmd.exe", out var filePath);
            Assert.Equal(true, found);
            Assert.Equal(@"C:\Windows\System32\cmd.exe", filePath, ignoreCase: true);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            var found = OsSettingsResolver.TryFindInEnvPath("sh", out var filePath);
            Assert.Equal(true, found);
            Assert.NotNull(filePath);
        }
    }

    [Fact]
    public void EnvVar()
    {
        var launchArguments = new LaunchArguments(
            Left: (temp, target) => string.Empty,
            Right: (temp, target) => string.Empty);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        var found = OsSettingsResolver.Resolve(
            new(Windows: new("ComSpec", "cmd.exe", launchArguments, "")),
            out var filePath,
            out var launchArgs);
        Assert.Equal(true, found);
        Assert.Equal(@"C:\Windows\System32\cmd.exe", filePath, ignoreCase: true);
    }

    public OsSettingsResolverTest(ITestOutputHelper output) :
        base(output)
    {
    }
}