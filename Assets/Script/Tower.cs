using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefab của mũi tên
    public Transform firePoint;          // Vị trí bắn mũi tên
    public float attackRange = 5f;       // Tầm bắn
    public float attackRate = 1f;        // Tốc độ bắn (số lần bắn mỗi giây)
    public int damage = 10;              // Sát thương mỗi lần bắn

    private float nextAttackTime = 0f;
    private GameObject target;           // Quái vật mục tiêu

    void Update()
    {
        // Kiểm tra tầm bắn và bắn
        if (Time.time >= nextAttackTime)
        {
            GameObject nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null)
            {
                target = nearestEnemy;
                //RotateTowardTarget(target);  // Xoay súng về hướng quái vật
                Shoot(target);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Tìm quái vật gần nhất
    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(firePoint.position, enemy.transform.position);  // Dùng Vector3.Distance thay cho Vector2
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }

        return nearestEnemy;
    }

    // Bắn mũi tên về phía quái vật
    void Shoot(GameObject target)
    {
        if (target != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTarget(target);
            }
        }
    }

    //void RotateTowardTarget(GameObject enemy)
    //{
    //    // Tính toán hướng đến quái vật
    //    Vector3 directionToEnemy = enemy.transform.position - firePoint.position;

    //    // Đảm bảo rằng hướng di chuyển không thay đổi trên trục y nếu không muốn xoay trên trục Y
    //    directionToEnemy.y = 0;  // Hoặc không cần nếu bạn muốn có sự xoay trên trục Y

    //    // Tạo một quaternion xoay về hướng quái vật
    //    Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

    //    // Xoay về hướng quái vật
    //    transform.rotation = Quaternion.RotateTowards(firePoint.rotation, targetRotation, Time.deltaTime * 500f);
    //}



    // Hiển thị vòng tầm bắn
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(firePoint.position, attackRange);  // Vẽ vòng tròn quanh điểm bắn
    }
}
