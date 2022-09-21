using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePooling : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPref, spawnPoint;
    [SerializeField]
    int amountToPool;
    [SerializeField]
    List<GameObject> pooledBullet;
    void Start()
    {
        pooledBullet = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject tmp = Instantiate(bulletPref, spawnPoint.transform.position, spawnPoint.transform.rotation);
            tmp.SetActive(false);
            pooledBullet.Add(tmp);
        }
    }
    private void Update()
    {
        for (int i = 0; i < pooledBullet.Count; i++)
        {
            if (!pooledBullet[i].activeInHierarchy)
            {
                pooledBullet[i].transform.position = spawnPoint.transform.position;
                pooledBullet[i].transform.rotation = spawnPoint.transform.rotation;
            }
        }
    }
    public void GetPooledBullet()
    {
        for (int i = 0; i < pooledBullet.Count; i++)
        {
            if (!pooledBullet[i].activeInHierarchy)
            {
                pooledBullet[i].transform.position = spawnPoint.transform.position;
                pooledBullet[i].transform.rotation = spawnPoint.transform.rotation;

                if (pooledBullet[i].transform.position == spawnPoint.transform.position)
                {
                    //pooledBullet[i].GetComponent<TrailRenderer>().Clear();
                    pooledBullet[i].SetActive(true);
                }
                break;
            }
        }
    }
}
