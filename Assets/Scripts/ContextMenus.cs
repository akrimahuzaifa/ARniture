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
            if (item != transform && item.GetComponent<ButtonManager>() == null)
            {
                item.AddComponent<Button>();
                item.AddComponent<ButtonManager>();
                item.GetComponent<ButtonManager>().btn = item.GetComponent<Button>();
                //item.GetComponent<ButtonManager>().furniture = Resources.Load<GameObject>("Perfabs-Models/" + item.gameObject.name);
#if UNITY_EDITOR
                var obj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Perfabs-Models/" + item.gameObject.name + ".prefab", typeof(GameObject));
#endif
                //item.GetComponent<ButtonManager>().furniture = obj;
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
            if (item.GetComponent<ButtonManager>() != null)
            {
                DestroyImmediate(item.GetComponent<ButtonManager>());
                DestroyImmediate(item.GetComponent<Button>());
            }
        }
    }
}
