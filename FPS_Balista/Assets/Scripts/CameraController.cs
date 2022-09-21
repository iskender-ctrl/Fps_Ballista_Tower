using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;
public class CameraController : MonoBehaviour
{
    public float h = 2.0F;
    public float v = 2.0F;
    [SerializeField]
    float yaw = 0f;
    float pitch = 0f;
    Vector3 FirstPoint;
    Vector3 SecondPoint;
    Vector3 EndPoint;
    public Animator anim, rollerAnim, indicator;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;
    bool unTouchable = false;
    bool unMoveButton = true;
    public float minClampAngle;
    public float maxClampAngle;
    public bool xAngleClamp;
    bool isHandless, isDesktop;
    [SerializeField] float horizontalSpeed = 20;
    [SerializeField] float verticalSpeed = 20;
    [SerializeField] float nextFire = 0;
    [SerializeField] float weaponFrequency = 0.5f;
    [SerializeField] GameObject[] spikes;
    [SerializeField] GameObject roller, MODEL, binok, smokes;
    [SerializeField] ParticleSystem smokeParticle;
    [SerializeField] ParticleSystem[] smokeParticles;
    [SerializeField] bool level3, level4;
    Shaker shaker;
    int clickRightMouse;
    [SerializeField] ShakeData myShake;
    void Start()
    {
        shaker = GameObject.FindObjectOfType<Shaker>();
        anim = gameObject.transform.parent.GetComponent<Animator>();
        rollerAnim = roller.gameObject.GetComponent<Animator>();
#if UNITY_STANDALONE_WIN
        isDesktop = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    }
    void Update()
    {
        /*if (isHandless)
        {
            TouchController();
        }*/

        if (isDesktop)
        {
            if (!Cursor.visible)
            {
                yaw += horizontalSpeed * Time.deltaTime * h * Input.GetAxis("Mouse X");
                pitch -= verticalSpeed * Time.deltaTime * v * Input.GetAxis("Mouse Y");
                pitch = Mathf.Clamp(pitch, minClampAngle, maxClampAngle);
                transform.eulerAngles = new Vector3(pitch, yaw, 0);

                if (Input.GetAxis("Mouse X") > 0)
                {
                    anim.SetBool("Right", true);
                    anim.SetBool("Left", false);
                }
                else if (Input.GetAxis("Mouse X") < 0)
                {
                    anim.SetBool("Right", false);
                    anim.SetBool("Left", true);
                }
            }

            DesktopFire();

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("HandFire") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                anim.SetBool("Fire", false);
            }
        }
        else
        {
            TouchController();
        }
        
        if (binok.activeInHierarchy)
        {
            MODEL.SetActive(false);
            smokes.SetActive(false);
        }
        else
        {
            MODEL.SetActive(true);
            smokes.SetActive(true);
        }

        if (indicator.GetCurrentAnimatorStateInfo(0).IsName("IndicatorComeBack") && indicator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f)
        {
            indicator.SetBool("ComeBack", false);
        }
    }

    void DesktopFire()
    {
        if (indicator.GetCurrentAnimatorStateInfo(0).IsName("Indicator1"))
        {
            if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
            {
                CameraShakerHandler.Shake(myShake);
                indicator.SetBool("ComeBack", true);
                indicator.speed = 1;
                shaker.RecoilFire();
                nextFire = Time.time + weaponFrequency;
                anim.SetBool("Fire", true);
                rollerAnim.SetBool("RollerFire", true);
                rollerAnim.speed = 1;
                GetComponent<SpikePooling>().GetPooledBullet();
                smokeParticle.Play();

                if (level4 || level3)
                {
                    for (int i = 0; i < smokeParticles.Length; i++)
                    {
                        smokeParticles[i].Play();
                    }
                }
                //spikes[timer].SetActive(false);
                //timer++;

                /* if (timer == spikes.Length)
                {
                    roller.transform.localRotation = Quaternion.Euler(-90, 0, 0);
                    for (int i = 0; i < spikes.Length; i++)
                    {
                        spikes[i].SetActive(true);
                        timer = 0;
                    }
                }*/

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag == "Head")
                    {
                        print(hit.collider.tag);
                        //hit.transform.GetComponent<Zombie>().health = 0;
                    }

                    if (hit.collider.tag == "Zombie")
                    {
                        print(hit.collider.tag);
                        //hit.transform.GetComponent<Zombie>().health -= damage;
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && level4 || Input.GetMouseButtonDown(1) && level3)
        {
            if (clickRightMouse == 0)
            {
                Camera.main.GetComponent<Camera>().fieldOfView = 30;
                binok.SetActive(true);
                clickRightMouse++;
            }
            else
            {
                Camera.main.GetComponent<Camera>().fieldOfView = 60;
                binok.SetActive(false);
                clickRightMouse = 0;
            }
        }
    }
    public void MobileFire()
    {
        if (indicator.GetCurrentAnimatorStateInfo(0).IsName("Indicator1"))
        {
            if (Time.time > nextFire)
            {
                indicator.SetBool("ComeBack", true);
                shaker.RecoilFire();
                nextFire = Time.time + weaponFrequency;
                anim.SetBool("Fire", true);
                rollerAnim.SetBool("RollerFire", true);
                rollerAnim.speed = 1;
                GetComponent<SpikePooling>().GetPooledBullet();
                smokeParticle.Play();

                if (level4 || level3)
                {
                    for (int i = 0; i < smokeParticles.Length; i++)
                    {
                        smokeParticles[i].Play();
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider.tag == "Head")
                    {
                        print(hit.collider.tag);
                        //hit.transform.GetComponent<Zombie>().health = 0;
                    }

                    if (hit.collider.tag == "Zombie")
                    {
                        print(hit.collider.tag);
                        //hit.transform.GetComponent<Zombie>().health -= damage;
                    }
                }
            }
        }
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
                    anim.SetBool("Right", true);
                    anim.SetBool("Left", false);
                }
                else
                {
                    anim.SetBool("Right", false);
                    anim.SetBool("Left", true);
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
    public void AnimationPause()
    {
        rollerAnim.speed = 0;
    }
}
