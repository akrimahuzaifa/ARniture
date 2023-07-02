using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorWallButtonsHandler : MonoBehaviour
{
    [SerializeField] private Transform btnsHolder;
    [SerializeField] private Transform inActivebtnsHolder;

    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] private DataHandler dataHandler;

    private void Reset()
    {
        btnsHolder = FindObjectOfType<UIContentFitter>(true).transform;
        inActivebtnsHolder = btnsHolder.parent.GetChild(1);
        
        buttonManager = GetComponent<ButtonManager>();
        dataHandler = FindObjectOfType<DataHandler>();
    }

    private void Start()
    {
        buttonManager.onClick.AddListener(() => OnSelectCategory(gameObject.name));
    }

    private void OnSelectCategory(string btnName)
    {
        for (int i = 0; i < btnsHolder.childCount; i++)
        {
            btnsHolder.GetChild(i).SetParent(inActivebtnsHolder.transform);
        }


        if (btnName.Contains("WallObjects"))
        {
            for (int i = 0; i < dataHandler.wallObjects.Count; i++)
            {
                inActivebtnsHolder.GetChild(0).SetParent(btnsHolder);
                ObjectButtonHandler btn = btnsHolder.GetChild(i).GetComponent<ObjectButtonHandler>();
                btn.name = dataHandler.wallObjects[i].name;
                btn.ItemId = i;
                btn.ButtonTexture = dataHandler.wallObjects[i].itemImage;
                btn.isFurniture = false;
            }
        }
        else
        {
            for (int i = 0; i < dataHandler.furnitures.Count; i++)
            {
                Debug.Log("ChildCount: " + inActivebtnsHolder.childCount);
                if (inActivebtnsHolder.childCount > 0)
                {
                    inActivebtnsHolder.GetChild(0).SetParent(btnsHolder);
                }
                ObjectButtonHandler btn = btnsHolder.GetChild(i).GetComponent<ObjectButtonHandler>();
                btn.name = dataHandler.furnitures[i].name;
                btn.ItemId = i;
                btn.ButtonTexture = dataHandler.furnitures[i].itemImage;
                btn.isFurniture = true;
            }
        }
        UIContentFitter.Instance.StartContentSizeFitter();
    }
}
