using System;
using UnityEngine;

public enum Stats
{
    damage,
    speed,
    amount,
}

public class Ability : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected float cooldown = 1f;
    [SerializeField] protected float damage = 1f;
    [SerializeField] protected float range = 3f;
    [SerializeField] protected int level = 0;
    [SerializeField] protected Transform owner;

    protected float cooldownTimer;

    protected virtual void Awake()
    {
        if (owner == null)
        {
            owner = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public virtual void LevelUp(Stats stat)
    {
        level++;
    }

    protected void ApplyDamage(Collider2D target)
    {
        if (target == null)
        {
            return;
        }

        IDamageable damageable = target.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            return;
        }
    }
}
