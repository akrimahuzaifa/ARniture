#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectHandler))]
public class InspectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUIStyle style = new GUIStyle()
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,

            normal = new GUIStyleState()
            {
                background = Texture2D.whiteTexture
            },
            hover = new GUIStyleState()
            {
                background = Texture2D.grayTexture
            },
            active = new GUIStyleState()
            {
                background = Texture2D.blackTexture
            }
        };

        ObjectHandler myScript = (ObjectHandler)target;

        if (GUILayout.Button("Reset", style))
        {
            myScript.OnResetButton();
        }
    }
}
#endif
