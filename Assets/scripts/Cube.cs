using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    private ObjectPool<GameObject> _pool;
    private Spawner _spawner;
    private bool _hasCollised = false;

    public bool HasCollised => _hasCollised;

    private void OnCollisionEnter(Collision collision)
    {
        _spawner.DestroyCube(this, collision);
    }

    private void ResetState()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Renderer>().material.color = Color.white;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _hasCollised = false;
    }

    public void ChangeStatus()
    {
        _hasCollised = true;
    }

    public void ReturnToPool()
    {
        ResetState();

        _pool.Release(gameObject);
    }

    public void Initialize(Spawner spawner, ObjectPool<GameObject> pool)
    {
        if (_spawner != null) 
            return;

        _pool = pool;
        _spawner = spawner;
    }
}
