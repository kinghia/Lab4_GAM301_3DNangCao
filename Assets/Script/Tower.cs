using UnityEngine;

public class Tower : MonoBehaviour
{
    public ObjectPool projectilePool;
    public ObjectPool vfxPool;
    public Transform firePoint;
    public float attackRange = 5f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            GameObject nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
            {
                Shoot(nearestEnemy);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    GameObject GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(firePoint.position, enemy.transform.position);
            if (distance < shortestDistance && distance <= attackRange)
            {
                nearestEnemy = enemy;
                shortestDistance = distance;
            }
        }

        return nearestEnemy;
    }

    void Shoot(GameObject target)
    {
        GameObject projectile = projectilePool.GetObject(firePoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.SetTarget(target, projectilePool, vfxPool);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, attackRange);
    }
}
