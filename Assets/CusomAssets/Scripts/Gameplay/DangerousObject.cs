using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousObject : MonoBehaviour
{
    [SerializeField] int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        var other = collision.collider;
        var health = other.GetComponentInParent<HealthComponent>();
        if (health == null) return;
        health.SetDamage(this.damage);
    }
}
