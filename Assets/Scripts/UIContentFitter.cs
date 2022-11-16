using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIContentFitter : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ContentSizeFitter());
    }

    IEnumerator ContentSizeFitter()
    {
        yield return new WaitForSeconds(3f);
        HorizontalLayoutGroup hg = GetComponent<HorizontalLayoutGroup>();
        int childCount = transform.childCount - 1;
        float childWidth = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        float width = hg.spacing * childCount + childCount * childWidth + hg.padding.left;
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, 230);
    }
}
