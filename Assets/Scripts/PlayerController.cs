using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Vector2 _jumpDirection;
    [SerializeField] Vector2 _flyDirection;
    [SerializeField] float _minPower = 1f;
    [SerializeField] float _maxPower = 100f;
    [SerializeField] int _flyLeftCount = 10;
    [SerializeField] float _decreaseSpeed = 1f;
    [SerializeField] float _holdDownSpeed = -1f;

    Rigidbody2D _myRigidbody;
    Vector2 holdVelocity;

    public bool _isStart;
    float startTime, endTime;

    void Start()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        JumpStart();
        Fly();
        Hold();
    }

    private void Hold()
    {
        if (!_isStart) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            holdVelocity = _myRigidbody.velocity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
            if (_myRigidbody.velocity.y > 0f)
            {
                _myRigidbody.velocity = new Vector2(holdVelocity.x, holdVelocity.y - _decreaseSpeed * Time.deltaTime);
                Debug.Log(_myRigidbody.velocity.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(holdVelocity.x, _holdDownSpeed);
            }
        }
    }

    void JumpStart()
    {
        if (_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTime = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            endTime = Time.time;
            float elapsedTime = Mathf.Clamp(endTime - startTime, _minPower, _maxPower);
            Debug.Log(elapsedTime);
            _myRigidbody.AddForce(elapsedTime * _jumpDirection, ForceMode2D.Impulse);
            _isStart = true;
        }
    }

    void Fly()
    {
        if(_flyLeftCount <= 0 || !_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _myRigidbody.AddForce(_flyDirection, ForceMode2D.Impulse);
            _flyLeftCount--;
        }
    }

}
