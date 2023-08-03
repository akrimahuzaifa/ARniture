using Michsky.MUIP;
using System.Collections.Generic;
using UnityEngine;

public class SpecificObject : MonoBehaviour
{
    [SerializeField] private Transform btnsHolder;
    [SerializeField] private Transform inActiveBtnsHolder;

    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private ButtonManager buttonManager;

    [SerializeField] private bool isFurniture;

    private void Reset()
    {
        btnsHolder = FindObjectOfType<UIContentFitter>(true).transform;
        inActiveBtnsHolder = btnsHolder.parent.GetChild(1);

        dataHandler = FindObjectOfType<DataHandler>();
        buttonManager = GetComponent<ButtonManager>();
    }

    private void Start()
    {
        isFurniture = transform.parent.parent.parent.parent.name.ToLower().Contains("floor");
        buttonManager.onClick.AddListener(() => OnSelectBtn(gameObject.name));
    }

    private void OnSelectBtn(string btnName)
    {
        //Debug.Log("Specific Btn Click btnsHolder.childCount" + btnsHolder.childCount);
        int btnsCount = btnsHolder.childCount;
        for (int i = 0; i < btnsCount; i++)
        {
            //Debug.Log("Specific btnsHolder.childCount: " + i);
            btnsHolder.GetChild(0).SetParent(inActiveBtnsHolder.transform);
        }

        SearchBtnInCategory(btnName, dataHandler.furnitures);
        SearchBtnInCategory(btnName, dataHandler.wallObjects);
        UIContentFitter.Instance.StartContentSizeFitter();
    }

    private void SearchBtnInCategory(string btnName, List<Item> objList)
    {
        int counter = 0;
        for (int i = 0; i < objList.Count; i++)
        {
            if (objList[i].name.Contains(btnName))
            {
                inActiveBtnsHolder.GetChild(0).SetParent(btnsHolder);
                ObjectButtonHandler btn = btnsHolder.GetChild(counter).GetComponent<ObjectButtonHandler>();
                btn.name = objList[i].name;
                btn.ItemId = i;
                btn.ButtonTexture = objList[i].itemImage;
                btn.isFurniture = isFurniture;
                counter++;
            }
        }
    }
}
