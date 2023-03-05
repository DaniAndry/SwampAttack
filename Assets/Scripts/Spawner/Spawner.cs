using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Player _player;

    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private float _timeAfterLastSpawn;
    private int _spawned;

    public event UnityAction AllEnemySpawned;

    public void NextWave()
    {
        SetWave(_currentWaveNumber);
        _spawned = 0;
    }

    private void Start()
    {
        SetWave(_currentWaveNumber);
    }

    private void Update()
    {
        if (_currentWave == null)
            return;

        _timeAfterLastSpawn += Time.deltaTime;

        if (_timeAfterLastSpawn >= _currentWave.Delay && _currentWave.Count != _spawned)
        {
            InstantiateEnemy();
            _spawned++;
            _timeAfterLastSpawn = 0;
        }

        if (_currentWave.Count <= _spawned)
        {
            if (_waves.Count > _currentWaveNumber + 1)
            {
                AllEnemySpawned?.Invoke();
            }
            _currentWave = null;
        }
    }

    private void InstantiateEnemy()
    {
        if (_currentWave.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, _currentWave.Count-1);
            GameObject Template = _currentWave.Templates[index];
            Enemy enemy = Instantiate(Template, _spawnPoint.position, _spawnPoint.rotation, _spawnPoint).GetComponent<Enemy>();

            enemy.Init(_player);
            enemy.Dying += OnEnemyDying;
        }
    }

    private void OnEnemyDying(Enemy enemy)
    {
        enemy.Dying -= OnEnemyDying;

        _player.AddMoney(enemy.Reward);

    }

    private void SetWave(int index)
    {
        _currentWave = _waves[index];
    }
}

[Serializable]
public class Wave
{
    public List<GameObject> Templates;
    public float Delay;
    public int Count;
}
