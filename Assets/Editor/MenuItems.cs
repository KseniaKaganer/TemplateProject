using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MenuItems
{
	//button name
	//hotkey you can use the following special characters: % (ctrl), # (shift), & (alt)
	//is button visible
	//button priority
	[MenuItem("MyMenu/Start Demo1 %#q", false, 1)]
    private static void StartSimulation()
    {
        EditorSceneManager.OpenScene("Assets/_Project/Scenes/Demo1.unity");
        EditorApplication.isPlaying = true;
    }

	[MenuItem("MyMenu/Start Demo2 %#w", true, 52)]
    private static void LoadConfigScene()
    {
        EditorSceneManager.OpenScene("Assets/_Project/Scenes/Demo2.unity");
    }
		



}
