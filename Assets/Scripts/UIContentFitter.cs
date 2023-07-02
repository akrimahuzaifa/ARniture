using cakeslice;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIContentFitter : MonoBehaviour
{
    public static UIContentFitter instance;
    public static UIContentFitter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIContentFitter>();
            }
            return instance;
        }
    }

    private void Start()
    {
        //StartCoroutine(ContentSizeFitter());
    }

    public IEnumerator ContentSizeFitter()
    {
        yield return new WaitForSeconds(1f);
        HorizontalLayoutGroup hg = GetComponent<HorizontalLayoutGroup>();
        int childCount = transform.childCount - 1;
        float childWidth = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        float width = hg.spacing * childCount + childCount * childWidth + hg.padding.left;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, 230);
    }

    public void StartContentSizeFitter()
    {
        StartCoroutine(ContentSizeFitter());
    }
}
