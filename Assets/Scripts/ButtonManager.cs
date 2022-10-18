using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button btn;
    [SerializeField] RawImage buttonImage;

    int itemId;
    public int ItemId 
    {
        set => itemId = value; 
    }

    Sprite buttonTexture;
    public Sprite ButtonTexture
    {
        set
        {
            buttonTexture = value;
            buttonImage.texture = buttonTexture.texture;
        }
    }


    void Start()
    {
        btn.onClick.AddListener(() => SelectedObject());
    }

    private void Update()
    {
        if (ScriptsRef.Instance.UImanager.OnEntered(gameObject))
        {
            transform.DOScale(Vector3.one * 2, 0.3f);
            //transform.localScale = Vector3.one * 2;
        }
        else
        {
            transform.DOScale(Vector3.one, 0.3f);
            //transform.localScale = Vector3.one;
        }
    }

    public void SelectedObject()
    {
        Debug.Log("Clicked item: " + gameObject.name);
        //DataHandler.Instance.furniture = furniture;
        DataHandler.Instance.SetFurniture(itemId);
    }
}
