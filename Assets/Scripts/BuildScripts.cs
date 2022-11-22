//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;

//public class BuildScript : MonoBehaviour
//{

//    [UnityEditor.MenuItem("MyMenu/MyBuild//buiiiild", false, 1)]
//    static void PerformBuild()
//    {
//        string curDir = Directory.GetCurrentDirectory() + "\\Build\\";
//        Directory.CreateDirectory(curDir);//curDir 경로 생성
//        string[] scene = { "Assets/Scenes/TitleScene.unity", "Assets/Scenes/LoadingScene.unity", "Assets/Scenes/GameScene.unity" };
//        BuildPipeline.BuildPlayer(scene,curDir+"test.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
//    }
//}