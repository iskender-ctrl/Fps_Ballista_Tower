using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    bool isFire = true;
    Rigidbody rb;
    [SerializeField] float speed, gravity;
    public int damage;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFire)
        {
            transform.Translate(0, speed * Time.deltaTime, -gravity * Time.deltaTime);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Plane")
        {
            isFire = false;
            rb.isKinematic = true;
            GetComponent<Animator>().enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            //print("Zombiye girdi");
            //other.gameObject.SetActive(false);
        }
    }
}
