#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public static class BuildScript
{
    // Adjust if you prefer a different folder/name
    private const string OutputDir = "Builds/Smoke";

    [MenuItem("CI/Build/Windows")]
    public static void BuildWindows()
    {
        var scenes = new[] { "Assets/Scenes/Smoke.unity" };
        Directory.CreateDirectory(OutputDir);
        BuildPipeline.BuildPlayer(scenes, Path.Combine(OutputDir, "Kingdoms_Smoke.exe"),
            BuildTarget.StandaloneWindows64, BuildOptions.None);
    }

    [MenuItem("CI/Build/macOS")]
    public static void BuildMac()
    {
        var scenes = new[] { "Assets/Scenes/Smoke.unity" };
        Directory.CreateDirectory(OutputDir);
        BuildPipeline.BuildPlayer(scenes, Path.Combine(OutputDir, "Kingdoms_Smoke.app"),
            BuildTarget.StandaloneOSX, BuildOptions.None);
    }
}
#endif
