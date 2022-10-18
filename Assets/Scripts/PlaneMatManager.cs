using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneMatManager : MonoBehaviour
{
    public Material planeMat;
    public Button[] planeTextureButtons;

    private void Reset()
    {
        planeTextureButtons = GameObject.Find("TextureHolderPanel").GetComponentsInChildren<Button>();
    }

    private void Awake()
    {
        foreach (var item in planeTextureButtons)
        {
            Texture tex = item.transform.Find("Mask/RawImage").GetComponent<RawImage>().texture;
            item.onClick.AddListener(() => OnClickTexButton(tex));
        }
    }


    void OnClickTexButton(Texture tex)
    {
        planeMat.mainTexture = tex;
    }

}
