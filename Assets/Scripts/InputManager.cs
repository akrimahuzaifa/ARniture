using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class InputManager : ARBaseGestureInteractable
{
    [SerializeField] Camera arCam;
    [SerializeField] GameObject crosshair;
    [SerializeField] GameObject arObj;
    [SerializeField] ARRaycastManager _ARRaycastManager;
    [SerializeField] private List<GameObject> _ARObjects = new List<GameObject>();
    public GameObject PreviewObject;
    Touch touch;
    Pose pose;
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    protected override bool CanStartManipulationForGesture(TapGesture gesture)
    {
        if (gesture.targetObject == null)
            return true;
        return false;
    }

    protected override void OnEndManipulation(TapGesture gesture)
    {
        if (gesture.isCanceled)
            return;
        if (gesture.targetObject != null || IsPointerOverUI(gesture))
        {
            return;
        }
        if (GestureTransformationUtility.Raycast(gesture.startPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            PreviewObject = Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
            Debug.Log("Object instantiated...: " + PreviewObject.name);
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;
            PreviewObject.transform.parent = anchorObject.transform;

            #region Logic to do not instantiate new object if its already there
            /*            var furniture = DataHandler.Instance.GetFurniture();
                        if (_ARObjects.Contains(furniture))
                        {
                            var objOpen = (from fur in _ARObjects
                                           where fur == furniture
                                           select fur).FirstOrDefault();

                            var obj = _ARObjects.Where(x => x == furniture).FirstOrDefault();
                            obj.transform.parent.SetPositionAndRotation(pose.position, pose.rotation);
                        }
                        else
                        {
                            GameObject PreviewObject = Instantiate(furniture, pose.position, pose.rotation);
                            PreviewObject.name = DataHandler.Instance.GetFurniture().name;
                            Debug.Log("Object instantiated...: " + PreviewObject.name);
                            var anchorObject = new GameObject("PlacementAnchor");
                            anchorObject.transform.position = pose.position;
                            anchorObject.transform.rotation = pose.rotation;
                            PreviewObject.transform.parent = anchorObject.transform;
                            _ARObjects.Add(PreviewObject);
                        }*/
            #endregion
        }
    }

    private void Reset()
    {
#if UNITY_EDITOR
        arObj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Models/Perfabs-Models/Sofa.prefab", typeof(GameObject));
#endif
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        _ARRaycastManager = FindObjectOfType<ARRaycastManager>();
        crosshair = GameObject.Find("CrossHair");
    }

    private void FixedUpdate()
    {
        CrosshairCalculation();
        //-----Logic to move object with crossheir---
        //if (!PreviewObject) return;
        //PreviewObject.transform.position = crosshair.transform.position;
    

/*#if !UNITY_EDITOR
        CrosshairCalculation();
        touch = Input.GetTouch(0);
        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began) return;
        if (IsPointerOverUI(touch)) return;
        Instantiate(DataHandler.Instance.GetFurniture(), pose.position, pose.rotation);
#endif*/
    }

    bool IsPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    void CrosshairCalculation()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        if (GestureTransformationUtility.Raycast(origin, _hits, TrackableType.PlaneWithinPolygon))
        {
            pose = _hits[0].pose;
            crosshair.transform.position = pose.position;
            //crosshair.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
