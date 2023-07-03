using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorWallButtonsHandler : MonoBehaviour
{
    [SerializeField] private Transform btnsHolder;
    [SerializeField] private Transform inActiveBtnsHolder;

    [SerializeField] private DataHandler dataHandler;
    [SerializeField] private ButtonManager buttonManager;

    private void Reset()
    {
        btnsHolder = FindObjectOfType<UIContentFitter>(true).transform;
        inActiveBtnsHolder = btnsHolder.parent.GetChild(1);

        dataHandler = FindObjectOfType<DataHandler>();
        buttonManager = GetComponent<ButtonManager>();
    }

    private void Start()
    {
        buttonManager.onClick.AddListener(() => OnSelectCategory(gameObject.name));
    }

    private void OnSelectCategory(string btnName)
    {
        //ButtonsOnClicksHandler.OnCategoryClick?.Invoke(btnName);
        Debug.Log("Before btnsHolder.childCount" + btnsHolder.childCount);
        int btnsCount = btnsHolder.childCount;
        for (int i = 0; i < btnsCount; i++)
        {
            Debug.Log("btnsHolder.childCount: " + i);
            btnsHolder.GetChild(0).SetParent(inActiveBtnsHolder.transform);
        }

        if (btnName.Contains("WallObjects"))
        {
            OnClickSetBtns(dataHandler.wallObjects, false);
        }
        else
        {
            OnClickSetBtns(dataHandler.furnitures, true);
        }
        UIContentFitter.Instance.StartContentSizeFitter();
    }

    private void OnClickSetBtns(List<Item> objList, bool furnitureStatus)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            inActiveBtnsHolder.GetChild(0).SetParent(btnsHolder);
            ObjectButtonHandler btn = btnsHolder.GetChild(i).GetComponent<ObjectButtonHandler>();
            btn.name = objList[i].name;
            btn.ItemId = i;
            btn.ButtonTexture = objList[i].itemImage;
            btn.isFurniture = furnitureStatus;
        }
    }
}
