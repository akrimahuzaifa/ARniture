using cakeslice;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject ARSelection;
    [SerializeField] private GameObject onCollision;

    #region EditorCode
    private void Reset()
    {
        MakeOrGetObject("ARSelection", ref ARSelection, 0);
        MakeOrGetObject("OnCollision", ref onCollision, 1);

        GetComponent<ARSelectionInteractable>().selectionVisualization = ARSelection;

#if UNITY_EDITOR
        //var collider = GetComponent<BoxCollider>();
        //collider.material = AssetDatabase.LoadAssetAtPath<PhysicMaterial>("Assets/Prefabs/ObjectPhysics.physicMaterial");
#endif
        var rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;

    }

    private void MakeOrGetObject(string name, ref GameObject setObject, int colorCode)
    {
        GameObject mesh = transform.GetChild(0).gameObject;
        Debug.Log("Mesh: " + mesh.name);
        if (!transform.Find(name))
        {
            Debug.Log("NotFound making New");
            GameObject parent = new(name);
            parent.transform.parent = transform;
            GameObject child = Instantiate(mesh, parent.transform);
            child.name = mesh.name;
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

    public void OnResetButton()
    {
        //Debug.Log("Button clicked!");
        Reset();
    }
    #endregion EditorCode

    private void OnCollisionEnter(Collision collision)
    {
        onCollision.SetActive(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollision.SetActive(false);
    }
}

