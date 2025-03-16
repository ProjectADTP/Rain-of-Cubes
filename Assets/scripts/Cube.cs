using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Cube : MonoBehaviour
{
    const float Mass = 100;

    private Spawner _spawner;
    private bool _hasCollised = false;

    private Collider _collider;
    private Rigidbody _rigidbody;
    private Renderer _color;

    public bool HasCollised => _hasCollised;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _color = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!HasCollised && collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            ChangeStatus();
            _spawner.DestroyCube(this);
        }
    }

    private void OnDisable()
    {
        _spawner.ReturnObjectToPool(gameObject);
    }

    public void ResetState()
    {
        _collider.enabled = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.mass = Mass;
        _color.material.color = Color.white;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _hasCollised = false;
    }

    public void ChangeStatus()
    {
        _hasCollised = true;
    }

    public void Initialize(Spawner spawner)
    {
        if (_spawner != null) 
            return;

        _spawner = spawner;
    }
}
