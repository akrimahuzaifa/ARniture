using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DataHandler : MonoBehaviour
{
    GameObject furniture;
    [SerializeField] ButtonManager buttonPrefab;
    [SerializeField] GameObject buttonContainer;
    [SerializeField] List<Item> items;
    [SerializeField] private string label;
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
#if UNITY_EDITOR
        buttonPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + "Button.prefab", typeof(GameObject)).GetComponent<ButtonManager>();
#endif
        buttonContainer = GameObject.Find("Content");
    }

    private async void Awake()
    {
        items = new List<Item>();
        //LoadItems();
        await Get(label); 
        CreateButtons();
    }

    void CreateButtons()
    {
        foreach (Item i in items)
        {
            ButtonManager b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.name = i.name;
            b.ItemId = currentId;
            b.ButtonTexture = i.itemImage;
            currentId++;
        }
        StartCoroutine(UIContentFitter.Instance.ContentSizeFitter());
    }

/*    void LoadItems()
    {
        var itemsObj = Resources.LoadAll("Items", typeof(Item));
        foreach (var item in itemsObj)
        {
            items.Add(item as Item);
        }
    }*/

    public void SetFurniture(int id)
    {
        furniture = items[id].itemPrefab;
    }

    public GameObject GetFurniture()
    {
        return furniture;
    }

    public async Task Get(string label)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        foreach (var location in locations)
        {
            var obj = await Addressables.LoadAssetAsync<Item>(location).Task;
            items.Add(obj);
        }
    }
}
