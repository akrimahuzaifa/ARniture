using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject arObj;
    [SerializeField] Camera arCam;
    [SerializeField] ARRaycastManager _ARRaycastManager;
    [SerializeField] GameObject crosshair;
    Touch touch;
    Pose pose;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void Reset()
    {
#if UNITY_EDITOR
        arObj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Models/Perfabs-Models/Sofa.prefab", typeof(GameObject));
#endif
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        _ARRaycastManager = FindObjectOfType<ARRaycastManager>();
        crosshair = GameObject.Find("CrossHair");
    }

    private void Update()
    {
#if !UNITY_EDITOR
        CrosshairCalculation();
        touch = Input.GetTouch(0);
        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began) return;
        if (IsPointerOverUI(touch)) return;
        Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
#endif
    }

    bool IsPointerOverUI(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void CrosshairCalculation()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        Ray ray = arCam.ScreenPointToRay(origin);
        if (_ARRaycastManager.Raycast(ray, _hits))
        {
            pose = _hits[0].pose;
            crosshair.transform.position = pose.position;
            //crosshair.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
