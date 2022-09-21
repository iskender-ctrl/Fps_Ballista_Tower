using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isDesktop, isHandless;
    [SerializeField] GameObject desktop, mobile;
    CameraController cameraController;
    int clickCount;
    bool clickFireButton;
    void Start()
    {
        cameraController = GameObject.FindObjectOfType<CameraController>();
#if UNITY_STANDALONE_WIN
        isDesktop = true;
#endif
#if UNITY_WEBGL
        isDesktop = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif

#if UNITY_ANDROID
        isHandless = true;
#endif

#if UNITY_IOS
    isHandless = true;
#endif

        if (isDesktop)
        {
            desktop.SetActive(true);
        }

        if (isHandless)
        {
            mobile.SetActive(true);
        }
    }
    private void Update()
    {
        if (clickFireButton)
        {
            cameraController.MobileFire();
        }
        else
        {
            cameraController.anim.SetBool("Fire", false);
        }
    }
    public void FireButtonDown()
    {
        clickFireButton = true;
    }
    public void FireButtonUp()
    {
        clickFireButton = false;
    }
    public void Scope(GameObject Binok)
    {
        if (clickCount == 0)
        {
            Binok.SetActive(true);
            Camera.main.GetComponent<Camera>().fieldOfView = 30;
            clickCount++;
        }
        else
        {
            Binok.SetActive(false);
            Camera.main.GetComponent<Camera>().fieldOfView = 60;
            clickCount = 0;
        }
    }
}
