using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GoToTitle : EditorWindow
{
    [MenuItem("PIL Tools/Go To Title Screen", priority = 100)]
    static void Go()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/title.unity", OpenSceneMode.Single);
    }
}
