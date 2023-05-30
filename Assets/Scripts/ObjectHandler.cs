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
    private void Reset()
    {
        var selection = GetComponent<ARSelectionInteractable>();
        selection.selectionVisualization = gameObject;

        var collider = GetComponent<BoxCollider>();
#if UNITY_EDITOR
        collider.material = AssetDatabase.LoadAssetAtPath<PhysicMaterial>("Assets/Prefabs/ObjectPhysics.physicMaterial");
#endif
        var rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

    }
}
