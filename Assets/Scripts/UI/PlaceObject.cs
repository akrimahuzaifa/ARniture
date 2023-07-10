using Michsky.MUIP;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using static UnityEngine.XR.ARSubsystems.XRCpuImage;

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
        inputManager.gameObject.SetActive(true);
        inputManager.PreviewObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        var objHandler = inputManager.PreviewObject.GetComponent<ObjectHandler>();
        objHandler.normal.SetActive(true);
        objHandler.onCollision.SetActive(false);
        //DestroyComponents();
        inputManager.PreviewObject = null;
        gameObject.SetActive(false);
    }

    private void DestroyComponents()
    {
        Destroy(inputManager.PreviewObject.GetComponent<ObjectHandler>());
        Destroy(inputManager.PreviewObject.GetComponent<ARSelectionInteractable>());
        Destroy(inputManager.PreviewObject.GetComponent<ARTranslationInteractable>());
        Destroy(inputManager.PreviewObject.GetComponent<ARRotationInteractable>());
    }
}
