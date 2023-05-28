static partial class Implementation
{
    public static Definition SublimeMerge()
    {
        var launchArguments = new LaunchArguments(
            Left: (temp, target) => $"mergetool \"{target}\" \"{temp}\"",
            Right: (temp, target) => $"mergetool \"{temp}\" \"{target}\"");

        var environmentVariable = $"${DefaultEnvironmentVariablePrefix}_{nameof(DiffTool.SublimeMerge)}";
        return new(
            Tool: DiffTool.SublimeMerge,
            Url: "https://www.sublimemerge.com/",
            AutoRefresh: false,
            IsMdi: true,
            SupportsText: true,
            RequiresTarget: true,
            Cost: "Paid",
            BinaryExtensions: Array.Empty<string>(),
            OsSupport: new(
                Windows: new(
                    environmentVariable,
                    "smerge.exe",
                    launchArguments,
                    @"%ProgramFiles%\Sublime Merge\"),
                Linux: new(
                    environmentVariable,
                    "smerge",
                    launchArguments),
                Osx: new(
                    environmentVariable,
                    "smerge",
                    launchArguments,
                    "/Applications/smerge.app/Contents/MacOS/")),
            Notes: " * While SublimeMerge is not MDI, it is treated as MDI since it uses a single shared process to managing multiple windows. As such it is not possible to close a Sublime merge process for a specific diff. [Vote for this feature](https://github.com/sublimehq/sublime_merge/issues/1168)");
    }
}