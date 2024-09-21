using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float damage = 15f;
    [SerializeField] private float speed = 70f;
    [SerializeField] private GameObject impactEffect;
    private Vector3 startPoint;


    private Transform target;

    private void Start()
    {
        startPoint = transform.position;
    }


    public void SetTarget(Transform _target)
    {
        target = _target;
    }



    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Seek();

    }

    void Seek()
    {
        Vector3 dir = (target.position + new Vector3(0,2,0)) - transform.position;
        float dis = speed * Time.deltaTime;
        if(dir.magnitude <= dis)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * dis ,Space.World);

    }

    private void HitTarget()
    {
        GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
        effect.transform.LookAt(startPoint);
        Damage(target);
        Destroy(effect,2f);
        Destroy(gameObject);

    }

    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null) e.TakeDamage(damage);
    }
}
