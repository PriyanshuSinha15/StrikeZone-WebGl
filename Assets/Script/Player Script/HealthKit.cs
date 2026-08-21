using System;
using System.Collections;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField] private float healthBoost;

    private Coroutine lifeTimeCoroutine;

    private void OnEnable()
    {
        lifeTimeCoroutine = StartCoroutine(HealthKitLifeTime());
    }

    private void OnDisable()
    {
        if(lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
    }

    private IEnumerator HealthKitLifeTime()
    {
        yield return new WaitForSeconds(20f);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        GameController.instance.healthKitList.Remove(gameObject);

        HealthKitPool.instance.ReturnHealthKit(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerHealth>())
        {
            PlayerHealth.instance.IncreasePlayerHealth(healthBoost);
            GameplayUIManager.instance.SetPlayerHealthUI(PlayerHealth.instance.GetPlayerHealthRatio());
            GameController.instance.healthKitList.Remove(this.gameObject);
            HealthKitPool.instance.ReturnHealthKit(gameObject);
        }
    }
}
