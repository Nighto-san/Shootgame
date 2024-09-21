using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private DynamicJoystick joystick;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Attributes")]
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float focusRange = 20;
    [SerializeField] private float shootRate = 1;
    [SerializeField] private float shootCD = 0;
    [SerializeField] private RawImage hpbar;

    private Animator animator;
    private Transform target;
    private Vector3 direction;
    private Enemy enemy;
    private LineRenderer line;
    private AudioSource audio;

    private void Start()
    {

        animator = gameObject.GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        audio = GetComponent<AudioSource>();
        InvokeRepeating("TargetUpdate", 0f, 0.25f); //Поиск ближайшей цели 4 раза в секунду
    }

    void Update()
    {
        InputHandler();
        Move();
        UpdateAnimator();
        AimTarget();
        Laser();
        Shooting();
        
    }



    private void InputHandler()
    {
        Vector3 joystickDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        Vector3 keyboardDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = joystickDirection + keyboardDirection;
        if (direction.magnitude > 1) 
            direction.Normalize();


    }

    private void Move()
    {
        if (direction.magnitude >= 0.1f)
        {
            direction.Normalize();
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        }
            
    }

    void UpdateAnimator()
    {
        bool isMoving = direction.magnitude > 0;
        animator.SetBool("isMoving", isMoving);
    }

    void TargetUpdate()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); // Массив обьектов на сцене с тегом "Enemy"
        float nearestDistance = 1000f; GameObject nearestEnemy = null; // Обезопасим себя от ошибок 
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < nearestDistance) // определяем ближайшего врага
            {
                nearestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
            if (nearestEnemy != null && nearestDistance <= focusRange)
            {
                target = nearestEnemy.transform; // назначаем трансформ ближайшего врага как цель



            }
            else
            {
                target = null; 
            }

        }
    }

    private void AimTarget() // поворачиваемся к цели если она есть
    {
        if (target == null) return;

        transform.LookAt(target.transform.position);
    }

    private void OnDrawGizmos() // отображения дистанции фокуса в редакторе
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, focusRange);
    }

    private void Laser()
    {
        if (target == null) 
        {
            line.enabled = false;
            return;
        }
        line.enabled = true;
        line.SetPosition(0, gunPoint.position);
        line.SetPosition(1, target.position + new Vector3(0,2,0)); // целимся на 2 юнита выше
    }

    private void Shooting()
    {
        if (shootCD <= 0 && target != null)
        {
            Shoot();
            shootCD = shootRate;
        }
        shootCD -= Time.deltaTime;
    }

    private void Shoot()
    {
       GameObject bulletGO =  Instantiate(bulletPrefab,gunPoint.position,gunPoint.rotation);
       Bullet bullet = bulletGO.GetComponent<Bullet>();
        audio.Play();
       if (bullet != null)
        {
        bullet.SetTarget(target);
        }
    }

    private void UpdateLive(float amount)
    {
        PlayerStats.hp -= amount;
        float healthPercent = PlayerStats.hp / PlayerStats.maxHp;
        hpbar.rectTransform.localScale = new Vector3(healthPercent, 1, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                UpdateLive(enemy.damage);
                enemy.Die();

            }
        }
    }


}
