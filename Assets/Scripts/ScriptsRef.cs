using UnityEngine;

public class ScriptsRef : MonoBehaviour
{
    public static ScriptsRef instance;
    
    public UIManager UImanager;
    public ButtonManager ButtonManager;
    public InputManager InputManager;
    public DataHandler DataHandler;
    public PlaneMatManager PlaneMatManager;
    public ContextMenus ContextMenus;
    public CrossHairRotation CrossHairRotation;
    public UIContentFitter Fitter;
    public Item Item;

    public static ScriptsRef Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScriptsRef>(true);
            }
            return instance;
        }
    }

    private void Reset()
    {
        UImanager = FindObjectOfType<UIManager>(true);
        ButtonManager = FindObjectOfType<ButtonManager>(true);
        InputManager = FindObjectOfType<InputManager>(true);
        DataHandler = FindObjectOfType<DataHandler>(true);
        PlaneMatManager = FindObjectOfType<PlaneMatManager>(true);
        ContextMenus = FindObjectOfType<ContextMenus>(true);
        CrossHairRotation = FindObjectOfType<CrossHairRotation>(true);
        Fitter = FindObjectOfType<UIContentFitter>(true);
        Item = FindObjectOfType<Item>(true);
    }
}
