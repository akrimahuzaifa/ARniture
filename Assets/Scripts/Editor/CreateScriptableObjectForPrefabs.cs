using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateScriptableObjectForPrefabs : EditorWindow
{
    private string prefabFolderPath = "Assets/Prefabs/Perfabs-Models/Furnitures/";
    private string scriptableObjectFolderPath = "Assets/AddressableAssets/Items/";
    private bool searchInSubDirectories;

    [MenuItem("AHK/Create ScriptableObjects for Prefabs")]
    private static void ShowWindow()
    {
        GetWindow<CreateScriptableObjectForPrefabs>("Create ScriptableObjects");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create ScriptableObjects for Prefabs\n" +
                        "_____________________________________", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Get Prefab from Folder => Path:", EditorStyles.boldLabel);
        prefabFolderPath = EditorGUILayout.TextField(prefabFolderPath);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Create ScriptableObject in Folder => Path:", EditorStyles.boldLabel);
        scriptableObjectFolderPath = EditorGUILayout.TextField(scriptableObjectFolderPath);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Search in Sub-Directories By Parent Directory: ", EditorStyles.boldLabel);
        searchInSubDirectories = EditorGUILayout.Toggle(searchInSubDirectories);
        EditorGUILayout.Space();

        if (GUILayout.Button("Create ScriptableObjects"))
        {
            CreateScriptableObjects();
        }
    }

    private void CreateScriptableObjects()
    {
        if (searchInSubDirectories)
        {
            string[] prefabPaths = Directory.GetFiles(prefabFolderPath, "*.prefab", SearchOption.AllDirectories);
            foreach (string prefabPath in prefabPaths)
            {
                CreateAssetAtPath(prefabPath);
            }
        }
        else
        {
            // Get all the prefabs in the specified folder path
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabFolderPath });
            foreach (string prefabGuid in prefabGuids)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGuid);
                CreateAssetAtPath(prefabPath);
            }
        }
    }

    private void CreateAssetAtPath(string prefabPath)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        // Create a ScriptableObject with the same name as the prefab
        string fileName = Path.GetFileName(prefabPath);
        string scriptableObjectPath = scriptableObjectFolderPath + fileName;
        scriptableObjectPath = scriptableObjectPath.Replace(".prefab", ".asset");

        if (!File.Exists(scriptableObjectPath))
        {
            Item item = ScriptableObject.CreateInstance<Item>();
            AssetDatabase.CreateAsset(item, scriptableObjectPath);

            // Save the changes
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            SetItemVariables(prefabPath, scriptableObjectPath);
            //Debug.Log("Created ScriptableObject for prefab: " + prefab.name);
        }
        else
        {
            Debug.Log("File Already Exist!");
            SetItemVariables(prefabPath, scriptableObjectPath);
        }
    }

    private void SetItemVariables(string prefabPath, string scriptableObjectPath)
    {
        Item objToReset = AssetDatabase.LoadAssetAtPath<Item>(scriptableObjectPath);
        //Debug.Log("PrefabPath At Editor: " + prefabPath);
        //Debug.Log("searchInSubDirectories in Editor: " + searchInSubDirectories);
        //objToReset.prefabPath = prefabPath;
        objToReset.searchInSubDirectories = searchInSubDirectories;
        //Debug.Log("searchInSubDirectories in Item: " + objToReset.searchInSubDirectories);
        objToReset.Reset();
    }
}
