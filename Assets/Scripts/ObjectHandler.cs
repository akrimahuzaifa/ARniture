using cakeslice;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;

[
    //---AR Components---
    RequireComponent(typeof(ARSelectionInteractable), typeof(ARTranslationInteractable), typeof(ARRotationInteractable)), 
    //---others---
    RequireComponent(typeof(BoxCollider), typeof(Rigidbody))
]
public class ObjectHandler : MonoBehaviour
{
    public GameObject normal;
    public GameObject ARSelection;
    public GameObject onCollision;

    #region EditorCode
    private void Reset()
    {
#if UNITY_EDITOR
        MakeOrGetObject("ARSelection", ref ARSelection, 0);
        MakeOrGetObject("OnCollision", ref onCollision, 1);

        GetComponent<ARSelectionInteractable>().selectionVisualization = ARSelection;

        //var collider = GetComponent<BoxCollider>();
        //collider.material = AssetDatabase.LoadAssetAtPath<PhysicMaterial>("Assets/Prefabs/ObjectPhysics.physicMaterial");

        var rb = GetComponent<Rigidbody>();

        // Get the prefab path
        //var path = AssetDatabase.GetAssetPath(this); //Does not work in hirarchy

        string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);

        Debug.Log("Prefab Path: " + prefabPath);
        if (prefabPath.Contains("WallObjects"))
        {
            DestroyImmediate(GetComponent<ARRotationInteractable>());
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            rb.useGravity = true;
        }
        

        //ReSizeBoxCollider();
#endif
    }
    private void MakeOrGetObject(string name, ref GameObject setObject, int colorCode)
    {
        normal = transform.GetChild(0).gameObject;
        //Debug.Log("Normal: " + normal.name);
        if (!transform.Find(name))
        {
            Debug.Log("NotFound making New: " + name);
            GameObject parent = new(name);
            parent.transform.parent = transform;
            GameObject child = Instantiate(normal, parent.transform);
            child.name = normal.name;
            setObject = parent;
            ApplyShader(colorCode, parent);
            parent.SetActive(false);
        }
        else
        {
            Debug.Log("Already Have/Found: " + transform.Find(name).gameObject.name);
            setObject = transform.Find(name).gameObject;
            ApplyShader(colorCode, setObject);
            setObject.SetActive(false);
        }
    }

    private void ApplyShader(int shaderColor, GameObject parent)
    {
        foreach (var item in parent.GetComponentsInChildren<MeshRenderer>())
        {
            OutlineShader shader = item.GetComponent<OutlineShader>();
            if (!shader)
            {
                shader = item.AddComponent<OutlineShader>();
            }
            shader.color = shaderColor;
            item.enabled = false;
        }
    }

    private void ReSizeBoxCollider() 
    {
        MeshFilter[] meshFilters = normal.GetComponentsInChildren<MeshFilter>();
        // Create an empty bounds to encapsulate all the mesh bounds
        Bounds combinedBounds = new Bounds();

        // Loop through all the mesh filters and expand the combined bounds
        foreach (MeshFilter filter in meshFilters)
        {
            combinedBounds.Encapsulate(filter.mesh.bounds);
        }
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        // Set the center of the Box Collider to the center of the combined bounds
        boxCollider.center = combinedBounds.center;

        // Set the size of the Box Collider to the size of the combined bounds
        boxCollider.size = combinedBounds.size;
    }

    public void OnResetButton()
    {
        //Debug.Log("Button clicked!");
        Reset();
    }
    #endregion EditorCode

/*    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Oncollision Enter");
        normal.SetActive(false);
        onCollision.SetActive(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Oncollision Exit");
        normal.SetActive(true);
        onCollision.SetActive(false);
    }*/
}

