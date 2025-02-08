using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1;
    private GameObject target;

    public void SetTarget(GameObject targetEnemy)
    {
        target = targetEnemy;
    }

    void Update()
    {
        if (target != null)
        {
            // Kiểm tra nếu mục tiêu đã chết (bị hủy)
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // Tính toán hướng di chuyển từ mũi tên đến mục tiêu
            Vector3 direction = target.transform.position - transform.position;

            // Đảm bảo trục Z không thay đổi
            direction.z = 0;

            // Di chuyển mũi tên về phía mục tiêu
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            // Kiểm tra nếu mũi tên đã đến gần mục tiêu
            if (Vector3.Distance(transform.position, target.transform.position) <= 0.1f)
            {
                // Gây sát thương cho quái vật nếu có component EnemyHealth
                if (target.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.TakeDamage(damage);
                }

                // Hủy mũi tên khi đến đích
                Destroy(gameObject);
            }
        }
        else
        {
            // Nếu target là null, hủy mũi tên ngay lập tức
            Destroy(gameObject);
        }
    }
}
