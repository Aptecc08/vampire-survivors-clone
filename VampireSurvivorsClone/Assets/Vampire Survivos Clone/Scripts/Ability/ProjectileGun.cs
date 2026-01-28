using System.Collections;
using UnityEngine;

public class ProjectileGun : Ability
{
    [Header("Projectile Ability")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;
    [SerializeField] private float projectileLifeTime = 3f;
    [SerializeField] private int amountOfProjectile = 1;
    [SerializeField] private float atackSpeed = 4f;
    [SerializeField] private string enemyTag = "Enemy";

    private Coroutine fireRoutine;

    private void OnEnable()
    {
        if (fireRoutine == null)
        {
            fireRoutine = StartCoroutine(FireLoop());
        }
    }

    private void OnDisable()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
            fireRoutine = null;
        }
    }

    private IEnumerator FireLoop()
    {
        while (true)
        {
            if (level > 0 && atackSpeed > 0f)
            {
                FireProjectiles();
                yield return new WaitForSeconds(atackSpeed);
                continue;
            }

            yield return null;
        }
    }

    private void FireProjectiles()
    {
        Vector3 origin = owner != null ? owner.position : transform.position;
        Transform target = FindClosestEnemy(origin);
        if (target == null)
        {
            return;
        }

        Vector2 direction = (target.position - origin).normalized;
        int count = Mathf.Max(0, amountOfProjectile);
        StartCoroutine(SpawnProjectilesSequentially(origin, direction, count));
    }

    private Transform FindClosestEnemy(Vector3 origin)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies == null || enemies.Length == 0)
        {
            return null;
        }

        Transform closest = null;
        float closestSqr = float.MaxValue;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            if (enemy == null)
            {
                continue;
            }

            float sqr = (enemy.transform.position - origin).sqrMagnitude;
            if (sqr < closestSqr)
            {
                closestSqr = sqr;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    private void SpawnProjectile(Vector3 origin, Vector2 direction)
    {
        if (projectilePrefab == null)
        {
            return;
        }

        GameObject instance = Instantiate(projectilePrefab, origin, Quaternion.identity);
        Rigidbody2D body = instance.GetComponent<Rigidbody2D>();
        if (body != null)
        {
            body.linearVelocity = direction * projectileSpeed;
        }
        else
        {
            Projectile projectile = instance.GetComponent<Projectile>();
            projectile.Initialize(direction, projectileSpeed, damage);
        }

        Destroy(instance, projectileLifeTime);
    }

    private IEnumerator SpawnProjectilesSequentially(Vector3 origin, Vector2 direction, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnProjectile(origin, direction);

            if (i < count - 1)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public override void LevelUp(Stats stat)
    {
        base.LevelUp(stat);

        switch (stat)
        {
            case Stats.damage:
                damage += 1f;
                break;
            case Stats.speed:
                atackSpeed = Mathf.Max(1f, atackSpeed - 0.1f);
                break;
            case Stats.amount:
                amountOfProjectile = Mathf.Min(5, amountOfProjectile + 1);
                break;
        }
    }
}
