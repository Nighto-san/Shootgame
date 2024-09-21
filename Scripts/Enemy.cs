using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private ParticleSystem deathEffect;
    [Header("Atributes")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float health = 50;
    [SerializeField] public float damage = 10;
    [SerializeField] private RawImage hpBar;
    private float maxHealth;


    void Start()
    {
        maxHealth = health + PlayerStats.killCount;
        moveSpeed = moveSpeed + PlayerStats.killCount / 10;
        GameObject e = Instantiate(enemyPrefab);
        e.transform.SetParent(transform, false);
        player = FindAnyObjectByType<Player>();

    }


    void Update()
    {

        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        transform.LookAt(player.transform);

    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHP();
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        PlayerStats.killCount++;
        ParticleSystem effect = Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(effect, 2f);
    }

    void UpdateHP()
    {
        float healthPercent = health/maxHealth;
        hpBar.rectTransform.localScale = new Vector3(healthPercent, 1, 1);
    }
}
