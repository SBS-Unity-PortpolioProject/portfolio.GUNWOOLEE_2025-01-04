using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineDisable : MonoBehaviour
{
    public GameObject targetObject; // 비활성화할 오브젝트

    public void HideObject()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);
        }
    }
}
