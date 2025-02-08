using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Transform Target;
    private NavMeshAgent agent;

    public GameObject gameOverPanel;

    public enum State { Moving, Jumping, SpeedBoosting }
    public State currentState = State.Moving;

    public float totalDistance; // Tổng khoảng cách
    public float travelledDistance; // Khoảng cách đã đi được
    private float boostDuration = 2f; // Thời gian tăng tốc
    private bool isBoosting = false;
    private float originalSpeed; // Lưu lại tốc độ ban đầu của quái vật
    private float jumpDistance = 2f; // Khoảng cách nhảy (có thể thay đổi tùy ý)

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed; // Lưu lại tốc độ ban đầu
        totalDistance = Vector3.Distance(transform.position, Target.position); // Tính tổng khoảng cách từ quái đến đích
    }

    void Update()
    {
        if (Target != null)
        {
            // Cập nhật khoảng cách đã đi
            travelledDistance = Vector3.Distance(transform.position, Target.position);

            // Kiểm tra khi quái vật đã đi được ⅓ quãng đường
            if (travelledDistance <= totalDistance*2 / 3 && currentState == State.Moving)
            {
                // Thực hiện hành động ngẫu nhiên khi quái đã đi được ⅓ quãng đường
                RandomAction();
            }

            // Nếu đang không tăng tốc, di chuyển quái tới đích
            if (!isBoosting)
            {
                agent.SetDestination(Target.position);
            }

            // Nếu quái đã tới đích
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                GameOver();
            }
        }
    }

    void RandomAction()
    {
        int randomAction = Random.Range(0, 2); // 0: Nhảy, 1: Tăng tốc
        if (randomAction == 0)
        {
            Jump();
        }
        else
        {
            StartCoroutine(SpeedBoost());
        }

        // Chuyển trạng thái của quái sang hành động khác
        currentState = (State)randomAction;
    }

    void Jump()
    {
        Debug.Log("Quái nhảy!");

        // Tính toán vector hướng đến mục tiêu
        Vector3 directionToTarget = (Target.position - transform.position).normalized;

        // Dịch chuyển quái vật lên phía trước một đoạn nhỏ
        Vector3 jumpPosition = transform.position + directionToTarget * jumpDistance;

        // Dịch chuyển quái vật đến vị trí mới
        transform.position = jumpPosition;

        // Lập lại vị trí đích cho NavMeshAgent để tiếp tục hành động sau khi nhảy
        agent.SetDestination(Target.position);
    }

    IEnumerator SpeedBoost()
    {
        Debug.Log("Tăng tốc gấp đôi!");
        isBoosting = true;
        agent.speed = originalSpeed * 2; // Tăng tốc độ lên gấp đôi
        yield return new WaitForSeconds(boostDuration); // Đợi 2 giây
        agent.speed = originalSpeed; // Khôi phục tốc độ ban đầu
        isBoosting = false;
    }

    void GameOver()
    {
        Debug.Log("Game Over! Quái đã đến đích!");
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0;
    }
}
