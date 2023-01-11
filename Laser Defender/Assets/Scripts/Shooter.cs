using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileLifeTime;
    [SerializeField] float baseFiringRate;

    [Header("Enemy AI")]
    [SerializeField] bool usingEnemyAI;
    [SerializeField] float firingRateVariance;
    [SerializeField] float minimumFiringRate;

    [HideInInspector] public bool isFiring;

    Coroutine firingCoroutine;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if (usingEnemyAI)
        {
            isFiring = true;
        }
    }

    private void Update()
    {
        Fire();
    }

    private void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if(!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject instance = Instantiate(projectilePrefab, 
                                              transform.position, 
                                              Quaternion.identity);

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifeTime);

            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance,
                                                      baseFiringRate + firingRateVariance);

            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, 
                                               minimumFiringRate,
                                               float.MaxValue);

            audioManager.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
}
