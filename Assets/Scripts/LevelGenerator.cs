using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public class PlatformData
    {
        public GameObject prefab;
        public int spawnWeight;
        public Vector2Int size; // в €чейках: например, (3,1) Ч ширина 3 клетки, высота 1
    }

    public List<PlatformData> platformPrefabs;
    public Vector2Int gridSize = new Vector2Int(10, 5);
    public Vector2 cellSize = new Vector2(5f, 3f);
    public Transform parent;

    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();
    private int totalWeight;

    void Start()
    {
        totalWeight = platformPrefabs.Sum(p => p.spawnWeight);
        SpawnPlatforms();
    }

    void SpawnPlatforms()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector2Int originCell = new Vector2Int(x, y);

                if (occupiedCells.Contains(originCell)) continue;
                if (Random.value < 0.5f) continue;

                PlatformData selected = GetWeightedRandomPlatform();
                if (selected == null) continue;

                // ѕровер€ем, влезает ли платформа и свободны ли €чейки
                if (!CanPlacePlatform(originCell, selected.size)) continue;

                // ¬ычисл€ем позицию
                Vector3 spawnPos = new Vector3(
                    originCell.x * cellSize.x,
                    originCell.y * cellSize.y,
                    0
                );

                Instantiate(selected.prefab, spawnPos, Quaternion.identity, parent);

                // ќтмечаем зан€тые €чейки
                MarkCellsOccupied(originCell, selected.size);
            }
        }
    }

    bool CanPlacePlatform(Vector2Int origin, Vector2Int size)
    {
        for (int dx = 0; dx < size.x; dx++)
        {
            for (int dy = 0; dy < size.y; dy++)
            {
                Vector2Int cell = new Vector2Int(origin.x + dx, origin.y + dy);

                if (cell.x >= gridSize.x || cell.y >= gridSize.y)
                    return false;

                if (occupiedCells.Contains(cell))
                    return false;
            }
        }
        return true;
    }

    void MarkCellsOccupied(Vector2Int origin, Vector2Int size)
    {
        for (int dx = 0; dx < size.x; dx++)
        {
            for (int dy = 0; dy < size.y; dy++)
            {
                occupiedCells.Add(new Vector2Int(origin.x + dx, origin.y + dy));
            }
        }
    }

    PlatformData GetWeightedRandomPlatform()
    {
        int rand = Random.Range(0, totalWeight);
        int cumulative = 0;

        foreach (var platform in platformPrefabs)
        {
            cumulative += platform.spawnWeight;
            if (rand < cumulative)
                return platform;
        }

        return null;
    }
}
