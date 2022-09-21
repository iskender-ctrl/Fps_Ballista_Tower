using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalistaController : MonoBehaviour
{
    [SerializeField] GameObject cameraRot, binok;
    [SerializeField] float speed = 0.005f;
    [SerializeField] bool isDesktop;
    // Update is called once per frame
    void Update()
    {
        if (isDesktop)
        {
            if (!binok.activeInHierarchy)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, cameraRot.transform.rotation, speed);
            }
            else
            {
                transform.rotation = cameraRot.transform.rotation;
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, cameraRot.transform.rotation, speed);
        }
    }
}
