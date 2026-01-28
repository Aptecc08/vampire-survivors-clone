using UnityEngine;
using System.Collections.Generic;

public class Orbit : Ability
{
    [Header("Orbit")]
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float rotationSpeed = 180f;

    private readonly List<Projectile> projectiles = new List<Projectile>();
    private Transform orbitCenter;
    private float currentAngle;

    protected override void Awake()
    {
        base.Awake();
        ResolveCenter();
    }

    private void OnEnable()
    {
        ResolveCenter();
        RebuildProjectiles();
    }

    private void OnDisable()
    {
        CleanupProjectiles();
    }

    private void Update()
    {
        if (!enabled)
        {
            return;
        }

        if (orbitCenter == null)
        {
            ResolveCenter();
            if (orbitCenter == null)
            {
                return;
            }
        }

        if (projectiles.Count == 0)
        {
            return;
        }

        currentAngle += rotationSpeed * Time.deltaTime;
        float angleStep = 360f / projectiles.Count;

        for (int i = 0; i < projectiles.Count; i++)
        {
            Projectile projectile = projectiles[i];
            if (projectile == null)
            {
                continue;
            }

            float angle = (currentAngle + angleStep * i) * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * range;
            projectile.transform.position = orbitCenter.position + offset;
        }
    }

    public void SetProjectileCount(int count)
    {
        projectileCount = Mathf.Max(0, count);
        RebuildProjectiles();
    }

    public void AddProjectiles(int amount)
    {
        SetProjectileCount(projectileCount + amount);
    }

    public void SetRotationSpeed(float degreesPerSecond)
    {
        rotationSpeed = degreesPerSecond;
    }

    public void AddRotationSpeed(float degreesPerSecond)
    {
        rotationSpeed += degreesPerSecond;
    }

    private void ResolveCenter()
    {
        if (owner != null)
        {
            orbitCenter = owner;
            return;
        }

        GameObject tagged = GameObject.FindGameObjectWithTag(targetTag);
        if (tagged != null)
        {
            orbitCenter = tagged.transform;
        }
    }

    private void RebuildProjectiles()
    {
        int targetCount = Mathf.Max(0, projectileCount);

        while (projectiles.Count > targetCount)
        {
            Projectile projectile = projectiles[projectiles.Count - 1];
            projectiles.RemoveAt(projectiles.Count - 1);
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }

        while (projectiles.Count < targetCount)
        {
            Projectile projectile = CreateProjectile();
            if (projectile != null)
            {
                projectiles.Add(projectile);
            }
            else
            {
                break;
            }
        }
    }

    private Projectile CreateProjectile()
    {
        GameObject instance;

        instance = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        Projectile projectile = instance.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetDamage(damage);
        }

        return projectile;
    }

    private void CleanupProjectiles()
    {
        for (int i = 0; i < projectiles.Count; i++)
        {
            Projectile projectile = projectiles[i];
            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }

        projectiles.Clear();
    }

    public override void LevelUp(Stats stat)
    {
        base.LevelUp(stat);

        switch (stat)
        {
            case Stats.damage:
                damage += 1f;
                for (int i = 0; i < projectiles.Count; i++)
                {
                    Projectile projectile = projectiles[i];
                    if (projectile != null)
                    {
                        projectile.SetDamage(damage);
                    }
                }
                break;
            case Stats.amount:
                projectileCount = Mathf.Min(8, projectileCount + 1);
                RebuildProjectiles();
                break;
            case Stats.speed:
                rotationSpeed += 10f;
                break;
        }
    }
}
