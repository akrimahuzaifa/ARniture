using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item1", menuName = "Add Item/Item")]
public class Item : ScriptableObject
{
    public float price;
    public GameObject itemPrefab;
    public Sprite itemImage;

    public bool searchInSubDirectories;
    public string prefabPath;
    
    [SerializeField] private string texturePath;
    [SerializeField] private string objectName;
    [SerializeField] private bool isFurniture = false;

    #if UNITY_EDITOR
    public void Reset()
    {
        price = 100;
        SetPaths();
        GetObjectName();
        GetItemPrefab();
        SetItemImage();
    }



    private void SetPaths()
    {
        if (isFurniture)
        {
            prefabPath = "Assets/Prefabs/Perfabs-Models/Furnitures/";
            texturePath = "Assets/Textures/Sprites/Buttons/";
        }
        else
        {
            if (Directory.Exists("Assets/Prefabs/Perfabs-Models/WallObjects"))
            {
                prefabPath = "Assets/Prefabs/Perfabs-Models/WallObjects/";
                texturePath = "Assets/Textures/Sprites/Buttons/WallObjects/";
                Debug.Log("Paths: " + prefabPath + "\n" + texturePath);
            }
        }
    }

    private void GetObjectName()
    {
        // Get the asset file path of the ScriptableObject
        string assetPath = AssetDatabase.GetAssetPath(this);
        Debug.Log("AssetPath: " + assetPath);

        // Get the file name without extension
        objectName = Path.GetFileNameWithoutExtension(assetPath);
        Debug.Log("ObjName: " + objectName);
    }

    private void GetItemPrefab()
    {
        if (isFurniture)
        {
            //Debug.Log("searchInSubDirectories: " + searchInSubDirectories);
            if (searchInSubDirectories)
            {
                //---getting all folders from Pole---
                string[] allFolders = Directory.GetDirectories(prefabPath);
                foreach (string folderPath in allFolders)
                {
                    //Debug.Log(folderPath);
                    //Debug.Log(objectName);
                    var folderName = Path.GetFileName(folderPath);
                    if (objectName.Contains(folderName))
                    {
                        SetPrefab(folderPath);
                    }
                }
            }
            else
            {
                //Debug.Log(prefabPath);
                SetPrefab(prefabPath);
            }
        }
        else
        {
            SetPrefab(prefabPath);
        }
    }

    private void SetPrefab(string folderPath)
    {
        var files = Directory.GetFiles(folderPath, "*.prefab");
        foreach (string file in files)
        {
            //Debug.Log("file: " + file);
            //Debug.Log("objName:: " + objectName);
/*            if (file.Contains(objectName))
            {
                itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);
            }*/
            if (Path.GetFileNameWithoutExtension(file) == objectName)
            {
                itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);
            }
        }
    }

    private void SetItemImage()
    {
        var files = Directory.GetFiles(texturePath, "*.png");
        foreach(string file in files)
        {
            //Debug.Log(file);
            //Debug.Log(objectName);
            if (Path.GetFileNameWithoutExtension(file) == objectName)
            {
                itemImage = AssetDatabase.LoadAssetAtPath<Sprite>(file);
            }
        }
    }
    #endif
}
