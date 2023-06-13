using UnityEngine;

public class CloseUIOnStart : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
