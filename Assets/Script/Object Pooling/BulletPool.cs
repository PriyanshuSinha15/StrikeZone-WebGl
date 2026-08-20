using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 30;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        CreateInitialPool();
    }

    private void CreateInitialPool()
    {
        for(int i = 0; i< initialPoolSize; i++)
        {
            CreateBullet();
        }
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform);

        bullet.SetActive(false);

        bulletPool.Enqueue(bullet);

        return bullet;
    }

    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        if(bulletPool.Count == 0)
        {
            CreateBullet();
        }

        GameObject bullet = bulletPool.Dequeue();

        bullet.transform.SetPositionAndRotation(position, rotation);
        bullet.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
