using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _countPoint;

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        List<int> selectedIndices = new List<int>();

        while (selectedIndices.Count < _countPoint && selectedIndices.Count < _spawnPoints.Length)
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);

            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
                Instantiate(_enemyPrefab, _spawnPoints[randomIndex].position, Quaternion.identity);
            }
        }
    }
}
