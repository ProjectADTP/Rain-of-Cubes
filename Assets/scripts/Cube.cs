using System;
using UnityEngine;


public class Cube : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _color;

    public event Action<Cube> OnCollisionWithObstacle;

    private bool _hasCollised = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _color = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollised == false && collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            ChangeStatus();

            OnCollisionWithObstacle?.Invoke(this);
        }
    }

    public void ResetState()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _color.material.color = Color.white;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        _hasCollised = false;
    }

    public void ChangeStatus()
    {
        _hasCollised = true;
    }
}
