using System;
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
    public static Action OnButtonClick;

    //Touch touch;
    Pose pose;
    [SerializeField] Camera arCam;
    [SerializeField] GameObject crosshair;
    //[SerializeField] GameObject arObj;
    public GameObject PreviewObject;
    //[SerializeField] private List<GameObject> _ARObjects = new List<GameObject>();
    List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    
    [SerializeField] ARRaycastManager _ARRaycastManager;
    [SerializeField] PlaceObject placeObject;

    private new void Reset()
    {
#if UNITY_EDITOR
        //arObj = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Models/Perfabs-Models/Sofa.prefab", typeof(GameObject));
#endif
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
        crosshair = GameObject.Find("CrossHair");
        _ARRaycastManager = FindObjectOfType<ARRaycastManager>();
        placeObject = FindObjectOfType<PlaceObject>();
    }

    private new void OnEnable()
    {
        OnButtonClick += InstantiateObject;
    }

    private new void OnDisable()
    {
        OnButtonClick -= InstantiateObject;
    }

    private void FixedUpdate()
    {
        CrosshairCalculation();
        //-----Logic to move object with crossheir---
        //if (!PreviewObject) return;
        //PreviewObject.transform.parent.position = crosshair.transform.position;
    

/*#if !UNITY_EDITOR
        CrosshairCalculation();
        touch = Input.GetTouch(0);
        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began) return;
        if (IsPointerOverUI(touch)) return;
        Instantiate(DataHandler.Instance.GetDesiredObject(), pose.position, pose.rotation);
#endif*/
    }

    #region Touch Work
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
/*            PreviewObject = Instantiate(DataHandler.Instance.GetDesiredObject(), pose.position, pose.rotation);
            Debug.Log("Object instantiated...: " + PreviewObject.name);
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;
            PreviewObject.transform.parent = anchorObject.transform;*/

            #region Logic to do not instantiate new object if its already there
            /*            var desiredObject = DataHandler.Instance.GetDesiredObject();
                        if (_ARObjects.Contains(desiredObject))
                        {
                            var objOpen = (from fur in _ARObjects
                                           where fur == desiredObject
                                           select fur).FirstOrDefault();

                            var obj = _ARObjects.Where(x => x == desiredObject).FirstOrDefault();
                            obj.transform.parent.SetPositionAndRotation(pose.position, pose.rotation);
                        }
                        else
                        {
                            GameObject PreviewObject = Instantiate(desiredObject, pose.position, pose.rotation);
                            PreviewObject.name = DataHandler.Instance.GetDesiredObject().name;
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

    bool IsPointerOverUI(TapGesture touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    #endregion Touch Work
    
    void CrosshairCalculation()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        if (_ARRaycastManager.Raycast(origin, _hits, TrackableType.PlaneWithinPolygon))
        {
            pose = _hits[0].pose;
            crosshair.transform.position = pose.position;

            var plane = _hits[0]. trackable as ARPlane;
            Vector3 planeUp = plane.normal;
            Vector3 objectUp = crosshair.transform.up;

            Quaternion rotation = Quaternion.FromToRotation(objectUp, planeUp);
            crosshair.transform.rotation = rotation * crosshair.transform.rotation;
            //crosshair.transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }

    private void InstantiateObject()
    {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        if (_ARRaycastManager.Raycast(origin, _hits, TrackableType.PlaneWithinPolygon))
        {
            pose = _hits[0].pose;
            var plane = _hits[0].trackable as ARPlane;
            
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;

            var furniture = DataHandler.Instance.GetDesiredObject();
            PreviewObject = Instantiate(furniture, pose.position, /*pose.rotation*/Quaternion.identity, anchorObject.transform);
            PreviewObject.name = furniture.name;
            Debug.Log("Object instantiated...: " + PreviewObject.name);
            
            if (plane.alignment == PlaneAlignment.Vertical)
            {
                // Handle vertical plane placement differently
                Debug.Log("Vertical Plane: " + PreviewObject.name);

                // Set a predefined rotation for vertical plane
                //PreviewObject.transform.Rotate(Vector3.right, 90); // Rotate by 90 degrees around the X-axis
                //---Partially working---
                //PreviewObject.transform.rotation = Quaternion.Euler(90, 0, 90); // Adjust as needed

                //===Wroking===
                Vector3 planeUp = plane.normal;
                Vector3 objectforward = PreviewObject.transform.forward;

                Quaternion rotation = Quaternion.FromToRotation(objectforward, planeUp);
                PreviewObject.transform.rotation = rotation * PreviewObject.transform.rotation;
                //===Wroking===
            }
            else
            {
                // Handle horizontal plane placement (default behavior)
                Debug.Log("Horizontal Plane");
            }
            placeObject.gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
