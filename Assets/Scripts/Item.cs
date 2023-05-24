using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

[CreateAssetMenu(fileName = "Item1", menuName = "Add Item/Item")]
public class Item : ScriptableObject
{
    public float price;
    public GameObject itemPrefab;
    public Sprite itemImage;

    [SerializeField] private bool isFurniture = true;
    [SerializeField] private bool searchInSubDirectories;
    [SerializeField] private string objectName;
    [SerializeField] private string prefabPath;
    [SerializeField] private string texturePath;

    #if UNITY_EDITOR
    public void Reset()
    {
        price = 100;
        SetPrefabPath();
        texturePath = "Assets/Textures/Sprites";
        GetObjectName();
        GetItemPrefab();
        SetItemImage();
    }



    private void SetPrefabPath()
    {
        if (isFurniture)
        {
            prefabPath = "Assets/Prefabs/Perfabs-Models/Furnitures/";
        }
        else
        {
            if (Directory.Exists("Assets/Prefabs/Perfabs-Models/WallObjects"))
            {
                prefabPath = "Assets/Prefabs/Perfabs-Models/WallObjects/";
            }
        }
    }

    private void GetObjectName()
    {
        // Get the asset file path of the ScriptableObject
        string assetPath = AssetDatabase.GetAssetPath(this);

        // Get the file name without extension
        objectName = Path.GetFileNameWithoutExtension(assetPath);
    }

    private void GetItemPrefab()
    {
        if (isFurniture)
        {
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
                        var files = Directory.GetFiles(folderPath, "*.prefab");
                        foreach (string file in files)
                        {
                            //Debug.Log(file);
                            //Debug.Log(objectName);
                            if (file.Contains(objectName))
                            {
                                itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(file);
                            }                               
                        }
                    }
                }
            }
            else
            {

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
            if (file.Contains(objectName))
            {
                itemImage = AssetDatabase.LoadAssetAtPath<Sprite>(file);
            }
        }
    }
    #endif
}
