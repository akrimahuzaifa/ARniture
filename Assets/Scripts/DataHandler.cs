using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class DataHandler : MonoBehaviour
{
    GameObject furniture;
    [SerializeField] ButtonManager buttonPrefab;
    [SerializeField] GameObject buttonContainer;
    [SerializeField] List<Item> items;
    int currentId = 0;

    public static DataHandler instance;
    public static DataHandler Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataHandler>();
            }
            return instance;
        }
    }

    private void Reset()
    {
        buttonPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + "Button.prefab", typeof(GameObject)).GetComponent<ButtonManager>();
        buttonContainer = GameObject.Find("Content");
    }

    private void Awake()
    {
        LoadItems();
        CreateButtons();
    }

    void CreateButtons()
    {
        foreach (Item i in items)
        {
            ButtonManager b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.ItemId = currentId;
            b.ButtonTexture = i.itemImage;
            currentId++;
        }
    }

    void LoadItems()
    {
        var itemsObj = Resources.LoadAll("Items", typeof(Item));
        foreach (var item in itemsObj)
        {
            items.Add(item as Item);
        }
    }

    public void SetFurniture(int id)
    {
        furniture = items[id].itemPrefab;
    }

    public GameObject GetFurniture()
    {
        return furniture;
    }
}
