using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Daehee
public class FollowCamera : MonoBehaviour
{
    public enum State
    {
        follow,
        back,
        dash,
        recover,
        toSpace,
        shake
    };

    #region PrivateVariables
    [SerializeField] float _backSize = 5f;
    [SerializeField] float _dashSize = 6f;
    [SerializeField] float _defaultSize = 7f;
    [SerializeField] float _spaceSize = 10f;
    [SerializeField] float _downSizeSpeed = -2.5f;

    Camera _camera;
    Transform _player;
    Vector3 _followPosition, _followDashPosition;
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
        _player = FindObjectOfType<PlayerController>().transform;    
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
                    DownSize(_backSize, _downSizeSpeed * Time.deltaTime);
                    break;
                case State.dash:
                    DownSize(_dashSize, _downSizeSpeed * 2f * Time.deltaTime);
                    break;
                case State.recover:
                    RecoverSize(upSizeSpeed * Time.deltaTime);
                    break;
                case State.toSpace:
                    UpSize(_spaceSize, upSizeSpeed * Time.deltaTime);
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

    void DownSize(float targetSize, float delta)
    {
        if(_camera.orthographicSize > targetSize)
        {
            _camera.orthographicSize += delta;
        }
        else
        {
            _camera.orthographicSize = targetSize;
        }
    }

    void UpSize(float targetSize, float delta)
    {
        if(_camera.orthographicSize < targetSize)
        {
            _camera.orthographicSize += delta;
        }
        else
        {
            _camera.orthographicSize = targetSize;
            _state = State.follow;
        }
    }

    void RecoverSize(float delta)
    {
        if(_camera.orthographicSize < _defaultSize)
        {
            _camera.orthographicSize += delta;
        }
        else if(_camera.orthographicSize > _defaultSize)
        {
            _camera.orthographicSize -= delta;
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
