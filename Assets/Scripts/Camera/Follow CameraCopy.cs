using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Daehee
public class FollowCameraCopy : MonoBehaviour
{
    public enum State
    {
        follow,
        back,
        recover,
        toSpace,
        shake
    };

    #region PrivateVariables
    [SerializeField] float _backSize = 5f;
    [SerializeField] float _defaultSize = 7f;
    [SerializeField] float _spaceSize = 10f;
    [SerializeField] float _downSizeSpeed = -2.5f;

    Camera _camera;
    Transform _player;
    Vector3 _followPosition;
    float upSizeSpeed;
    float shakeDuration = 1f;
    float shakeMagnitude = 0.2f;
    float elapsedShakeTime;
    #endregion

    #region PublicVariables
    public State _state = State.follow;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerControllerCopy>().transform;    
    }

    void Start()
    {
        _followPosition = new Vector3(11f, 0f, -10f);
        upSizeSpeed = _downSizeSpeed * -2f;
    }

    void LateUpdate()
    {
        MoveCamera();

    }

    void MoveCamera()
    {
        if (_state == State.shake)
        {
            Shake();
        }
        else
        {
            transform.position = _player.position + _followPosition;
            switch (_state)
            {
                case State.back:
                    DownSize(_backSize);
                    break;
                case State.recover:
                    RecoverSize();
                    break;
                case State.toSpace:
                    UpSize(_spaceSize);
                    break;
                default:
                    break;
            }
        }
    }

    void Shake()
    {
        if (elapsedShakeTime < shakeDuration)
        {
            transform.position = _followPosition + _player.position + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedShakeTime += Time.deltaTime;
        }
        else
        {
            SetState(State.follow);
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
    #endregion

    #region PublicMethods
    public void SetState(State state)
    {
        _state = state;
    }

    public void HitCameraEffect()
    {
        SetState(State.shake);
        elapsedShakeTime = 0;
    }
    #endregion
}
