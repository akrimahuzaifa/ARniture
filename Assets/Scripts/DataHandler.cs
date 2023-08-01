using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;


public class DataHandler : MonoBehaviour
{
    GameObject desiredObject;
    [SerializeField] ObjectButtonHandler buttonPrefab;
    [SerializeField] GameObject buttonContainer;
    //[SerializeField] List<Item> items;
    [SerializeField] public List<Item> furnitures = new List<Item>();
    [SerializeField] public List<Item> wallObjects = new List<Item>();
    [SerializeField] public List<Item> allObjects = new List<Item>();
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
        buttonPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/" + "ObjectButton.prefab", typeof(GameObject)).GetComponent<ObjectButtonHandler>();
        var furnitureFiles = Directory.GetFiles("Assets/AddressableAssets/Items/", "*.asset");
        var wallObjectsFiles = Directory.GetFiles("Assets/AddressableAssets/Items/WallObjects/", "*.asset");
        foreach (var file in furnitureFiles)
        {
            var item = AssetDatabase.LoadAssetAtPath<Item>(file);
            furnitures.Add(item);
            allObjects.Add(item);
        }

        foreach (var wallObject in wallObjectsFiles)
        {
            var item = AssetDatabase.LoadAssetAtPath<Item>(wallObject);
            wallObjects.Add(item);
            allObjects.Add(item);
        }
#endif
        buttonContainer = FindObjectOfType<UIContentFitter>().gameObject;
    }

    /*    private async void Awake()
        {
            items = new List<Item>();
            //ObjectDownloadComplete();
            await Get(label); 
            CreateButtons();
        }*/

    private void Awake()
    {
        StartCoroutine(CreateButtons(furnitures));
    }
    
    IEnumerator CreateButtons(List<Item> items)
    {
        foreach (Item i in items)
        {
            ObjectButtonHandler b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.name = i.name;
            b.ItemId = currentId;
            b.ButtonTexture = i.itemImage;
            currentId++;
            //Debug.Log("Butn Instantiated:: " + b, b.gameObject);
        }
        yield return StartCoroutine(UIContentFitter.Instance.ContentSizeFitter());

    }

/*    void CreateButtons()
    {
        foreach (Item i in items)
        {
            ObjectButtonHandler b = Instantiate(buttonPrefab, buttonContainer.transform);
            b.name = i.name;
            b.ItemId = currentId;
            b.ButtonTexture = i.itemImage;
            currentId++;
        }
        StartCoroutine(UIContentFitter.Instance.ContentSizeFitter());
    }*/

    public void SetFurniture(int id)
    {
        desiredObject = furnitures[id].itemPrefab;
    }

    public void SetWallObject(int id)
    {
        desiredObject = wallObjects[id].itemPrefab;
    }

    public GameObject GetDesiredObject()
    {
        return desiredObject;
    }

/*    public async Task Get(string label)
    {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        foreach (var location in locations)
        {
            var obj = await Addressables.LoadAssetAsync<Item>(location).Task;
            items.Add(obj);
        }
    }*/

/*    IEnumerator DownloadFurniture(string label)
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
        //CreateButtons();
    }*/

/*    private void ObjectDownloadComplete()
    {
        StartCoroutine(DownloadFurniture(label));
    }*/
}
