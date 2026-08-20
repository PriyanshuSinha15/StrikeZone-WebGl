using System.Collections;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    private const string EXPLOSION_TRIGGER = "explode";

    private Coroutine lifeTimeCoroutine;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.ResetTrigger("explode");

        anim.SetTrigger("explode");

        lifeTimeCoroutine = StartCoroutine(ExplosionLifeTime());
    }

    private void OnDisable()
    {
        anim.ResetTrigger("explode");

        if(lifeTimeCoroutine != null)
        {
            StopCoroutine(lifeTimeCoroutine);
            lifeTimeCoroutine = null;
        }
    }
    
    private IEnumerator ExplosionLifeTime()
    {
        yield return new WaitForSeconds(1.2f);

        ReturnToPool();
    }

    void ReturnToPool()
    {
        GameController.instance.explosionPrefabList.Remove(gameObject);

        ExplosionPool.instance.ReturnExpolsion(gameObject);
    }
}
