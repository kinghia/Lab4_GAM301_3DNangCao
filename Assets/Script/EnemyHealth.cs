using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 10;  // Máu của quái vật
    public int damageTaken = 0;  // Lượng sát thương đã nhận

    // Hàm nhận sát thương từ đạn
    public void TakeDamage(int damage)
    {
        health -= damage;  // Giảm máu của quái vật
        damageTaken += damage;  // Cập nhật lượng sát thương đã nhận

        if (health <= 0)
        {
            Die();  // Nếu máu <= 0, quái vật sẽ chết
        }
    }

    // Hàm xử lý khi quái vật chết
    void Die()
    {
        // Bạn có thể thêm hiệu ứng chết hoặc âm thanh tại đây
        Debug.Log(gameObject.name + " đã chết!");

        // Hủy quái vật khỏi game
        Destroy(gameObject);
    }
}
