using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f; // 이동 속도
    public float changeDirectionInterval = 3f; // 방향 전환 간격
    public float moveRadius = 10f; // 이동 반경

    private Vector3 targetDirection;
    private float timer;
    private Terrain terrain;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        SetRandomDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeDirectionInterval)
        {
            SetRandomDirection();
            timer = 0f;
        }

        Move();
    }

    void SetRandomDirection()
    {
        // 랜덤 방향 설정
        float randomAngle = Random.Range(0, 360);
        targetDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0, Mathf.Sin(randomAngle * Mathf.Deg2Rad));
    }

    void Move()
    {
        Vector3 newPosition = transform.position + targetDirection * moveSpeed * Time.deltaTime;

        // Terrain 경계를 벗어나지 않도록 제한
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;
        float halfWidth = terrainWidth / 2;
        float halfLength = terrainLength / 2;

        float clampedX = Mathf.Clamp(newPosition.x, terrain.transform.position.x, terrain.transform.position.x + terrainWidth);
        float clampedZ = Mathf.Clamp(newPosition.z, terrain.transform.position.z, terrain.transform.position.z + terrainLength);
        float clampedY = terrain.SampleHeight(new Vector3(clampedX, 0, clampedZ)) + terrain.transform.position.y;

        transform.position = new Vector3(clampedX, clampedY, clampedZ);
    }
}
