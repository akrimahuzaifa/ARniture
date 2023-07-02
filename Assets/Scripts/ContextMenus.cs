using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenus : MonoBehaviour
{
    //function made for (Models Buttons Panel)
    [ContextMenu("Add Button Manager with assign Values to Child Buttons")]
    public void AddBtnManagerScp() 
    {  
        var allBtns = gameObject.GetComponentsInChildren<Transform>();
        foreach (var item in allBtns)
        {
            if (item != transform && item.GetComponent<ObjectButtonHandler>() == null)
            {
                item.AddComponent<Button>();
                item.AddComponent<ObjectButtonHandler>();
                item.GetComponent<ObjectButtonHandler>().btn = item.GetComponent<Button>();
                //item.GetComponent<ObjectButtonHandler>().desiredObject = Resources.Load<GameObject>("Perfabs-Models/" + item.gameObject.name);
#if UNITY_EDITOR
                var obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Perfabs-Models/" + item.gameObject.name + ".prefab", typeof(GameObject));
#endif
                //item.GetComponent<ObjectButtonHandler>().desiredObject = obj;
            }
        }
    }

    //function made for (Models Buttons Panel)
    [ContextMenu("Remove Button Manager to Buttons")]
    public void RemoveBtnManagerScp()
    {
        var allBtns = gameObject.GetComponentsInChildren<Button>();
        foreach (var item in allBtns)
        {
            if (item.GetComponent<ObjectButtonHandler>() != null)
            {
                DestroyImmediate(item.GetComponent<ObjectButtonHandler>());
                DestroyImmediate(item.GetComponent<Button>());
            }
        }
    }
}
