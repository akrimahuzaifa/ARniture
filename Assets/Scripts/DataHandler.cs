using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class DataHandler : MonoBehaviour
{
    GameObject furniture;
    [SerializeField] ButtonManager buttonPrefab;
    [SerializeField] GameObject buttonContainer;
    [SerializeField] List<Item> items;
    [SerializeField] List<Item> Furnitures;
    [SerializeField] private string label = "Furnitures";
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
        //---Uncomment it when runing reset in editor ---
        //buttonPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + "Button.prefab", typeof(GameObject)).GetComponent<ButtonManager>();
        
#endif
        buttonContainer = GameObject.Find("Content");
    }

    private void GetFurnitures()
    {

    }

    private async void Awake()
    {
        items = new List<Item>();
        //ObjectDownloadComplete();
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

    IEnumerator DownloadFurniture(string label)
    {
        yield return Get(label);
        var operation = Addressables.LoadResourceLocationsAsync(label).Task;
        while (!operation.IsCompleted)
        {
            Debug.Log("Progress: " + Addressables.LoadResourceLocationsAsync(label).PercentComplete);
            var objj = Instantiate(new GameObject());
            objj.name = Addressables.LoadResourceLocationsAsync(label).PercentComplete.ToString();
            yield return null;
        }
        CreateButtons();
    }

    private void ObjectDownloadComplete()
    {
        StartCoroutine(DownloadFurniture(label));
    }
}
