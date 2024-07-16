using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] float _minSize = 5f;
    [SerializeField] float _defaultSize = 7f;
    [SerializeField] float _maxSize = 10f;
    [SerializeField] float _minX = 9f;
    [SerializeField] float _defaultX = 11f;

    Camera _camera;
    Transform _player;
    Vector3 _followPosition;
    State _state = State.follow;

    enum State
    {
        follow,
        changing
    };

    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerController>().transform;    
    }

    void Start()
    {
        _followPosition = new Vector3(11f, 0f, -10f);
    }

    void LateUpdate()
    {
        if (_state == State.follow)
        {
            transform.position = _player.position + _followPosition;
        }
        else 
        {
            transform.position = Vector3.MoveTowards(transform.position, _followPosition, Time.deltaTime);
            _camera.orthographicSize += _camera.orthographicSize > _defaultSize ? -Time.deltaTime : Time.deltaTime;
            if (transform.position == _followPosition && _camera.orthographicSize == _defaultSize)
            {
                _state = State.follow;
            }
        }
    }

    public void DownSize()
    {
        _state = State.changing;
        transform.position = Vector3.MoveTowards(transform.position, _player.position + new Vector3(_minX, 0f), Time.deltaTime);
        _camera.orthographicSize = Math.Clamp(_camera.orthographicSize - Time.deltaTime, _minSize, _defaultSize);
    }

    public void UpSize()
    {
        _state = State.changing;
        _camera.orthographicSize = Math.Clamp(_camera.orthographicSize + Time.deltaTime, _defaultSize, _maxSize);
    }
}
