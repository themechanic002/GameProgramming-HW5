using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 생성할 Enemy 프리팹
    public int enemyCount = 15; // 생성할 Enemy 수
    public float spawnRadius = 50f; // 스폰 반경

    private Terrain terrain;

    void Start()
    {
        terrain = Terrain.activeTerrain;
        if (terrain == null)
        {
            Debug.LogError("Terrain이 존재하지 않습니다.");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionOnTerrain();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionOnTerrain()
    {
        // Terrain의 크기 가져오기
        float terrainWidth = terrain.terrainData.size.x;
        float terrainLength = terrain.terrainData.size.z;

        // 랜덤 X, Z 위치 생성
        float randomX = Random.Range(0, terrainWidth) + terrain.transform.position.x;
        float randomZ = Random.Range(0, terrainLength) + terrain.transform.position.z;

        // 월드 좌표로 변환
        Vector3 worldPosition = terrain.transform.position + new Vector3(randomX, 0, randomZ);

        // 해당 위치의 높이 가져오기
        float y = terrain.SampleHeight(worldPosition) + terrain.transform.position.y;

        return new Vector3(worldPosition.x, y, worldPosition.z);
    }
}
