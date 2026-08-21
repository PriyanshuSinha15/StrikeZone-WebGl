using UnityEngine;
using System.Collections.Generic;
using System;
public class HealthKitPool : MonoBehaviour
{
    public static HealthKitPool instance;

    [SerializeField] private GameObject healthKitPrefab;
    [SerializeField] private int intialPoolSize = 40;

    private Queue<GameObject> healthKitPool = new Queue<GameObject>();

    void Awake()
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
        for(int i = 0; i< intialPoolSize; i++)
        {
            CreateHealthKit();
        }
    }

    private GameObject CreateHealthKit()
    {
        GameObject healthKit = Instantiate(healthKitPrefab, transform);

        healthKit.SetActive(false);
        healthKitPool.Enqueue(healthKit);

        return healthKit;
    }

    public GameObject GetHealthKit(Vector3 position, Quaternion rotation)
    {
        if(healthKitPool.Count == 0)
        {
            CreateHealthKit();
        }

        GameObject healthKit = healthKitPool.Dequeue();

        healthKit.transform.SetPositionAndRotation(position, rotation);
        healthKit.SetActive(true);

        return healthKit;
    }

    public void ReturnHealthKit(GameObject healthKit)
    {
        if (healthKit == null)
            return;

        if (!healthKit.activeSelf)
            return;

        healthKit.SetActive(false);
        healthKitPool.Enqueue(healthKit);
    }
}
