#if UNITY_EDITOR
using UnityEditor;
using System.IO;

public static class BuildScript
{
    [MenuItem("CI/Build/Windows")]
    public static void BuildWindows()
    {
        var scenes = new[] { "Assets/Scenes/Smoke.unity" };
        Directory.CreateDirectory("Builds/Smoke");
        BuildPipeline.BuildPlayer(scenes, "Builds/Smoke/Kingdoms_Smoke.exe",
            BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
#endif
