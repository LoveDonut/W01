using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Daehee
public class FollowCamera : MonoBehaviour
{
    [SerializeField] float _backSize = 5f;
    [SerializeField] float _defaultSize = 7f;
    [SerializeField] float _spaceSize = 10f;
    [SerializeField] float _downSizeSpeed = -2.5f;

    Camera _camera;
    Transform _player;
    Vector3 _followPosition;
    public State _state = State.follow;
    float upSizeSpeed;

    public enum State
    {
        follow,
        back,
        recover,
        toSpace
    };

    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerController>().transform;    
    }

    void Start()
    {
        _followPosition = new Vector3(11f, 0f, -10f);
        upSizeSpeed = _downSizeSpeed * -2f;
    }

    void LateUpdate()
    {
        transform.position = _player.position + _followPosition;
        if(_state == State.back)
        {
            DownSize(_backSize);
        }
        if(_state == State.recover)
        {
            RecoverSize();
        }
        if (_state == State.toSpace)
        {
            UpSize(_spaceSize);
        }
    }

    void DownSize(float targetSize)
    {
        if(_camera.orthographicSize > targetSize)
        {
            _camera.orthographicSize += _downSizeSpeed * Time.deltaTime;
        }
        else
        {
            _camera.orthographicSize = targetSize;
        }
    }

    void UpSize(float targetSize)
    {
        if(_camera.orthographicSize < targetSize)
        {
            _camera.orthographicSize += upSizeSpeed * Time.deltaTime;
        }
        else
        {
            _camera.orthographicSize = targetSize;
            _state = State.follow;
        }
    }

    void RecoverSize()
    {
        if(_camera.orthographicSize < _defaultSize)
        {
            _camera.orthographicSize += upSizeSpeed * Time.deltaTime;
        }
        else if(_camera.orthographicSize > _defaultSize)
        {
            _camera.orthographicSize -= upSizeSpeed * Time.deltaTime;
        }

        if(Mathf.Abs(_camera.orthographicSize - _defaultSize) < 0.1f)
        {
            SetState(State.follow);
        }
    }

    public void SetState(State state)
    {
        _state = state;
    }
}
