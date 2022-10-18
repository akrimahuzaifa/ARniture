using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public PointerEventData pointerEventData;
    public EventSystem eventSystem;
    public Transform selectionPoint;

    private void Reset()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        selectionPoint = GameObject.Find("SelectionPoint").transform;
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    private void Start()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = selectionPoint.position;
    }

    public bool OnEntered(GameObject button)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject == button)
            {
                return true;
            }
        }
        return false;
    }
}
