using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private int _poolSize;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private ColorChanger _colorChanger;

    private ObjectPool<GameObject> _cubePool;

    private void Start()
    {
        _cubePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_cubePrefab, this.transform),
            actionOnGet: cube => cube.SetActive(true),
            actionOnRelease: cube => cube.SetActive(false),
            actionOnDestroy: cube => Destroy(cube),
            collectionCheck: false,
            defaultCapacity: _poolSize,
            maxSize: _poolMaxSize
        );

        InvokeRepeating(nameof(SpawnCube), 0f, 1.5f);
    }

    private void SpawnCube()
    {
        Cube cube = _cubePool.Get().GetComponent<Cube>();

        if (cube != null) 
        {
            cube.transform.position = new Vector2(Random.Range(transform.position.x - 10,
                transform.position.x + 10), transform.position.y);

            cube.Initialize(this);
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay, Cube cube)
    {
        float elapsedTime = 0f;

        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;

            yield return null; 
        }

        cube.ResetState();
        cube.gameObject.SetActive(false);
    }

    public void DestroyCube(Cube cube)
    {
        float lifeTime = Random.Range(2f, 5f);

        _colorChanger.ChangeColor(cube);

        StartCoroutine(ReturnToPoolAfterDelay(lifeTime, cube));
    }

    public void ReturnObjectToPool(GameObject gameObject)
    {
        _cubePool.Release(gameObject);
    }
}
