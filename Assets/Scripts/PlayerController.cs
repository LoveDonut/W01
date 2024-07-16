using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class PlayerController : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] Vector2 _jumpDirection = new Vector2(20,40);
    [SerializeField] float _minPower = 1f;
    [SerializeField] float _maxPower = 3;
    [SerializeField] float backOffset = -6f;
    [SerializeField] float backSpeed = 5f;
    [SerializeField] float goSpeed = 20f;

    [Header("Fly")]
    [SerializeField] Vector2 _flyDirection = new Vector2(1,40);
    [SerializeField] float flyCost = 10f;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;

    [Header("Damage")]
    [SerializeField] float damageByTime = 2f;

    Rigidbody2D _myRigidbody;
    FollowCamera _followCamera;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;

    public float hp = 100f; 
    bool _isStart;
    float _startTime, _endTime;

    bool IsAlive => hp > 0;

    void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _followCamera = FindObjectOfType<FollowCamera>();
    }

    void Update()
    {
        if (!IsAlive) return;
        JumpStart();
        Fly();
        Hold();
        Damage(Time.deltaTime * damageByTime);
    }

    void Hold()
    {
        if (!_isStart) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            _holdVelocity = _myRigidbody.velocity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
            if (_myRigidbody.velocity.y > 0f)
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdVelocity.y);
                Debug.Log(_myRigidbody.velocity.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdDownSpeed);
            }
        }
    }

    void JumpStart()
    {
        if (_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startTime = Time.time;
            _jumpPosition = transform.position;
            _followCamera.SetState(FollowCamera.State.back);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if(transform.position.x > _jumpPosition.x + backOffset)
            {
                transform.position -= new Vector3(backSpeed * Time.deltaTime, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _endTime = Time.time;
            float elapsedTime = Mathf.Clamp(_endTime - _startTime, _minPower, _maxPower);
            Debug.Log(elapsedTime);
            StartCoroutine(GoJump(elapsedTime));
            _followCamera.SetState(FollowCamera.State.recover);
        }
    }

    void Fly()
    {
        if(hp < flyCost || !_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _myRigidbody.AddForce(_flyDirection, ForceMode2D.Impulse);
            hp -= flyCost;
        }
    }

    IEnumerator GoJump(float elapsedTime)
    {
        while (transform.position.x < _jumpPosition.x)
        {
            transform.position += new Vector3(goSpeed * Time.deltaTime, 0);

            yield return new WaitForEndOfFrame();
        }

        _myRigidbody.AddForce(elapsedTime * _jumpDirection, ForceMode2D.Impulse);
        _isStart = true;
    }

    public void Damage(float damage)
    {
        hp -= damage;
    }
}
