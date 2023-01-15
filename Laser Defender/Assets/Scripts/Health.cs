using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int health;
    [SerializeField] int score;
    [SerializeField] ParticleSystem hitEffect;

    [SerializeField] bool applyCameraShake;
    CameraShake cameraShake;

    AudioManager audioManager;
    ScoreManager scoreManager;
    LevelManager levelManager;

    public int GetHealth()
    {
        return health;
    }

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioManager = FindObjectOfType<AudioManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();

        if(damageDealer != null)
        {
            TakeDamage(damageDealer.GetDamage());
            PlayHitEffect();
            audioManager.PlayDamageClip();
            ShakeCamera();
            damageDealer.Hit();
        }
    }

    private void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isPlayer)
        {
            scoreManager.ModifyScore(score);
        }
        else
        {
            levelManager.LoadGameOver();
        }

        Destroy(gameObject);
    }

    private void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    private void ShakeCamera()
    {
        if(cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }
}
