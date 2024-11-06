using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float bulletSpeed;
    float bulletDamage;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;
    }

    public void SetSpeed(float Speed)
    {
        bulletSpeed = Speed;
    }

    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.stats.TakeDamage(bulletDamage);
        }

       Destroy(gameObject);
    }

}
