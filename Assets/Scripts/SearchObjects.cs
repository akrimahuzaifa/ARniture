using Michsky.MUIP;
using TMPro;
using UnityEngine;


public class SearchObjects : MonoBehaviour
{
    public GameObject magnifyGlass;
    public TMP_InputField searchInputField;
    public ButtonManager[] allButtons;

    private void Reset()
    {
        magnifyGlass = GameObject.Find("Magnifyglass");
        searchInputField = GameObject.Find("SearchBar-Input Field").GetComponent<TMP_InputField>();
        allButtons = GameObject.Find("InteriorObjectsPanel").GetComponentsInChildren<ButtonManager>();
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
        searchInputField.onValueChanged.AddListener(OnInputfieldValueChange);
    }

    private void OnDisable()
    {
        searchInputField.onSelect.RemoveListener((str) => OnInputFieldSelect(str));
        searchInputField.onDeselect.RemoveListener((str) => OnInputFieldDeselect(str));
        searchInputField.onValueChanged.RemoveListener(OnInputfieldValueChange);
    }

    private void OnInputfieldValueChange(string searchText)
    {
        searchText = searchText.ToLower();

        foreach (var item in allButtons)
        {
            string btnText = item.buttonText.ToLower();
            item.gameObject.SetActive(btnText.Contains(searchText));
        }
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
            foreach (var item in allButtons)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
