using System;
using System.Collections;
using UnityEngine;


public class Cube : MonoBehaviour
{
    [SerializeField] private float _minCubeLiveTime;
    [SerializeField] private float _maxCubeLiveTime;

    private ColorChanger _colorChanger;
    private Rigidbody _rigidbody;
    private Renderer _color;
    private bool _hasCollised = false;
    
    public event Action<Cube> Collided;

    private void Awake()
    {
        _colorChanger = new ColorChanger();
        _rigidbody = GetComponent<Rigidbody>();
        _color = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasCollised == false && collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            float lifeTime = UnityEngine.Random.Range(_minCubeLiveTime, _maxCubeLiveTime);

            ChangeStatus();

            StartCoroutine(ReturnToPool(lifeTime));
        }
    }

    private IEnumerator ReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);

        ResetState();

        Collided?.Invoke(this);
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
        _color.material.color = _colorChanger.ChangeColor();
        _hasCollised = true;
    }
}
