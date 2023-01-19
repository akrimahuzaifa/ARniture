using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SearchObjects : MonoBehaviour
{
    public GameObject magnifyGlass;
    public TMP_InputField searchInputField;

    private void Reset()
    {
        magnifyGlass = GameObject.Find("Magnifyglass");
        searchInputField = GameObject.Find("SearchBar-Input Field").GetComponent<TMP_InputField>();
    }

    void Start()
    {
       

       
/*        searchInputField.OnSelect.AddListener(
            delegate
            {
                poleChoose.Invoke(funcName, 0);
            });*/
    }

    private void OnEnable()
    {
        searchInputField.onSelect.AddListener((str) => OnInputFieldSelect(str));
        searchInputField.onDeselect.AddListener((str) => OnInputFieldDeselect(str));
    }

    private void OnDisable()
    {
        searchInputField.onSelect.RemoveListener((str) => OnInputFieldSelect(str));
        searchInputField.onDeselect.RemoveListener((str) => OnInputFieldDeselect(str));
    }

    public void OnInputFieldSelect(string str)
    {
        //Debug.Log(str);
        magnifyGlass.SetActive(false);
    }

    public void OnInputFieldDeselect(string str)
    {
        //Debug.Log("Deselect: " + str);
        if (searchInputField.text == "")
        {
            magnifyGlass.SetActive(true);
        }
    }
}
