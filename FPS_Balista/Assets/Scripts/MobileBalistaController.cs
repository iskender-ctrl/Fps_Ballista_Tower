using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileBalistaController : MonoBehaviour
{
    Vector3 FirstPoint;
    Vector3 SecondPoint;
    Vector3 EndPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;
    bool unTouchable = false;
    bool unMoveButton = true;
    public float minClampAngle;
    public float maxClampAngle;
    public bool xAngleClamp;
    CameraController cameraController;
    void Start()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchController();
    }
    void TouchController()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && !unTouchable)
            {
                if (EventSystem.current.currentSelectedGameObject)
                {
                    unMoveButton = true;
                }
                else
                {
                    unTouchable = true;
                    unMoveButton = false;
                    FirstPoint = Input.GetTouch(0).position;
                    xAngleTemp = xAngle;
                    yAngleTemp = yAngle;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved && !unMoveButton)
            {
                SecondPoint = Input.GetTouch(0).position;
                xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 / Screen.width;
                yAngle = yAngleTemp + (SecondPoint.y - FirstPoint.y) * -90 / Screen.height;
                yAngle = Mathf.Clamp(yAngle, minClampAngle, maxClampAngle);

                if (xAngle > 0)
                {
                    cameraController.anim.SetBool("Right", true);
                    cameraController.anim.SetBool("Left", false);
                }
                else
                {
                    cameraController.anim.SetBool("Right", false);
                    cameraController.anim.SetBool("Left", true);
                }

                if (xAngleClamp == true)
                {
                    xAngle = Mathf.Clamp(xAngle, -15.56f, 45.6f);
                }
                this.transform.localRotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                unTouchable = false;
                unMoveButton = true;
                xAngleTemp = xAngle;
                yAngleTemp = yAngle;
            }
        }
    }
}
