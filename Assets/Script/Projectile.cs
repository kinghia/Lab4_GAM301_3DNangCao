using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    private GameObject target;
    private ObjectPool pool;

    public VisualEffect explosionEffectPrefab; // Prefab của hiệu ứng nổ
    private ObjectPool vfxPool; // Pool cho hiệu ứng VFX

    public void SetTarget(GameObject targetEnemy, ObjectPool projectilePool, ObjectPool effectPool)
    {
        target = targetEnemy;
        pool = projectilePool;
        vfxPool = effectPool;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.z = 0;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            if (Vector3.Distance(transform.position, target.transform.position) <= 0.1f)
            {
                if (target.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(damage);
                }

                // Lấy hiệu ứng VFX từ pool và phát
                GameObject vfxObject = vfxPool.GetObject(transform.position, Quaternion.identity);
                VisualEffect vfxInstance = vfxObject.GetComponent<VisualEffect>();
                if (vfxInstance != null)
                {
                    vfxInstance.Play();
                }

                ReturnToPool();
            }
        }
        else
        {
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        target = null;
        if (pool != null)
        {
            pool.ReturnObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
