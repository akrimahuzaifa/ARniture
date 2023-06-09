using Michsky.MUIP;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    [SerializeField]private ButtonManager buttonManager;
    [SerializeField]private InputManager inputManager;

    private void Reset()
    {
        inputManager = FindObjectOfType<InputManager>();
        buttonManager = GetComponent<ButtonManager>();
    }

    private void Start()
    {
        buttonManager.onClick.AddListener(() => OnPlaceClick());
        gameObject.SetActive(false);
    }

    private void OnPlaceClick()
    {
        inputManager.PreviewObject = null;
        gameObject.SetActive(false);
    }
}
