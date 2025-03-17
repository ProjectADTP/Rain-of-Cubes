using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private ColorChanger _colorChanger;
    [SerializeField] private float _minCubeLiveTime;
    [SerializeField] private float _maxCubeLiveTime;
    [SerializeField] private float _spawnTime;

    private ObjectPool<Cube> _cubePool;
    private float _rangeSpawn;

    private void Start()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab, this.transform),
            actionOnGet: cube => cube.gameObject.SetActive(true),
            actionOnRelease: cube =>
            {
                cube.ResetState();
                cube.OnCollisionWithObstacle -= DestroyCube;
                cube.gameObject.SetActive(false);
            },
            actionOnDestroy: cube => Destroy(cube),
            collectionCheck: false,
            defaultCapacity: _poolSize,
            maxSize: _poolMaxSize
        );

        _rangeSpawn = 10;

        InvokeRepeating(nameof(SpawnCube), 0f, _spawnTime);
    }

    private void SpawnCube()
    {
        Cube cube = _cubePool.Get().GetComponent<Cube>();

        if (cube != null) 
        {
            cube.transform.position = new Vector2(Random.Range(transform.position.x - _rangeSpawn,
                transform.position.x + _rangeSpawn), transform.position.y);

            cube.OnCollisionWithObstacle += DestroyCube;
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay, Cube cube)
    {
        yield return new WaitForSeconds(delay);

        _cubePool.Release(cube);
    }

    public void DestroyCube(Cube cube)
    {
        float lifeTime = Random.Range(_minCubeLiveTime, _maxCubeLiveTime);

        _colorChanger.ChangeColor(cube);

        StartCoroutine(ReturnToPoolAfterDelay(lifeTime, cube));
    }
}
