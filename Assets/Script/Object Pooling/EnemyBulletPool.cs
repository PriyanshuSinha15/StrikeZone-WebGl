using UnityEngine;
using System.Collections.Generic;

public class EnemyBulletPool : MonoBehaviour
{
    public static EnemyBulletPool instance;

    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private int initialPoolSize = 60;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
        
    void Awake()
    {
        if (instance == null)
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

    void CreateInitialPool()
    {
        for(int i = 0;  i < initialPoolSize; i++)
        {
            CreateBullet();
        }
    }

    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(enemyBullet, transform);

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
