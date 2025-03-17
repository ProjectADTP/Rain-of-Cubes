using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private float _spawnTime;

    private ObjectPool<Cube> _cubePool;

    private float _rangeSpawn;

    private void Start()
    {
        _cubePool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cubePrefab, this.transform),
            actionOnGet: cube => cube.gameObject.SetActive(true),
            actionOnRelease: cube => cube.gameObject.SetActive(false),
            actionOnDestroy: cube => Destroy(cube),
            collectionCheck: false,
            defaultCapacity: _poolSize,
            maxSize: _poolMaxSize
        );

        _rangeSpawn = 10;

        StartCoroutine(SpawnCube(_spawnTime));
    }

    private IEnumerator SpawnCube(float delay)
    {
        while (true) 
        {
            yield return new WaitForSeconds(delay);

            Cube cube = _cubePool.Get();

            if (cube != null) 
            {
                cube.transform.position = new Vector2(Random.Range(transform.position.x - _rangeSpawn,
                    transform.position.x + _rangeSpawn), transform.position.y);

                cube.Collided += DestroyCube;
            }
        }
    }

    public void DestroyCube(Cube cube)
    {
        cube.Collided -= DestroyCube;
        _cubePool.Release(cube);
    }
}
