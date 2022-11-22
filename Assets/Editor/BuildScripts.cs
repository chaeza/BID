using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildScripts : MonoBehaviour
{
    static void MyPerformBuild()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "mygame.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
