using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    public static ExplosionPool instance;

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int intialPoolSize = 20;

    private Queue<GameObject> explosionPool = new Queue<GameObject>();
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
        for(int i = 0; i < intialPoolSize; i++)
        {
            CreateExplosion();
        }
    }

    private GameObject CreateExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform);

        explosion.SetActive(false);
        explosionPool.Enqueue(explosion);

        return explosion;
    }

    public GameObject GetExplosion(Vector3 position, Quaternion rotation)
    {
        if(explosionPool.Count == 0)
        {
            CreateExplosion();
        }

        GameObject explosion = explosionPool.Dequeue();

        explosion.transform.SetPositionAndRotation(position, rotation);
        explosion.SetActive(true);

        return explosion;
    }


    public void ReturnExpolsion(GameObject explosion)
    {
        if (explosion == null)
            return;

        if (!explosion.activeSelf)
            return;

        explosion.SetActive(false);

        explosionPool.Enqueue(explosion);
    }
}
