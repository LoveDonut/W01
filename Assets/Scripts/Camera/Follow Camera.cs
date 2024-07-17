using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Daehee
public class FollowCamera : MonoBehaviour
{
    enum CameraState
    {
        moveToSun,
        moveToPlayer,
        follow
    };

    #region PrivateVariables
    [SerializeField] float _backSize = 5f;
    [SerializeField] float _defaultSize = 7f;
    [SerializeField] float _spaceSize = 12f;
    [SerializeField] float _downSizeSpeed = -2.5f;
    [SerializeField] float _cameraMoveSpeed = 1f;
    [SerializeField] Vector3 _sunBelowPosition = new Vector3();
    [SerializeField] Transform _sunTransform;
    [SerializeField] float _delay = 2f;

    Camera _camera;
    PlayerController _player;
    PlayerState _playerState;
    Vector3 _followPosition, _followDashPosition;
    CameraState cameraState = CameraState.moveToSun;

    float upSizeSpeed;
    float shakeDuration = 1f;
    float shakeMagnitude = 0.2f;
    float elapsedShakeTime;
    #endregion

    #region PublicVariables
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerController>();
        _playerState = FindObjectOfType<PlayerState>();
    }

    void Start()
    {
        _sunBelowPosition += _sunTransform.position;
        _followPosition = new Vector3(12f, 0f, -10f);
        upSizeSpeed = _downSizeSpeed * -2f;
    }

    void LateUpdate()
    {
        if (cameraState == CameraState.moveToSun) 
        {
            if (isCameraNear(_sunBelowPosition))
            {
                StartCoroutine(LookUpSunDelay());
            }
            else
            {
                MoveTo(_sunBelowPosition);
            }
        }
        else if (cameraState == CameraState.moveToPlayer)
        {
            MoveTo(_player.transform.position + _followPosition);
            if (isCameraNear(_player.transform.position + _followPosition))
            {
                cameraState = CameraState.follow;
                _playerState.SetState(PlayerState.State.follow);
            }
        }
        else 
        {
            transform.position = _player.transform.position + _followPosition;
            MoveCamera();
        }

    }

    IEnumerator LookUpSunDelay()
    {
        yield return new WaitForSeconds(_delay);
        cameraState = CameraState.moveToPlayer;
    }

    void MoveTo(Vector3 objective)
    {
        Vector2 directionTo = (objective - transform.position).normalized;
        transform.position += (Vector3)directionTo * _cameraMoveSpeed;
    }

    bool isCameraNear(Vector3 objective)
    {
        if(Mathf.Abs(transform.position.x - objective.x) + Mathf.Abs(transform.position.y - objective.y) < 2f)
        {
            return true;
        }
        return false;
    }

    void MoveCamera()
    {
        if (_playerState._state == PlayerState.State.shake)
        {
            Shake();
        }
        else
        {
            switch (_playerState._state)
            {
                case PlayerState.State.back:
                    DownSize(_backSize, _downSizeSpeed * Time.deltaTime);
                    break;
                case PlayerState.State.dash:
                    float dashSize = _defaultSize - 1;
                    DownSize(dashSize, _downSizeSpeed * 2f * Time.deltaTime);
                    break;
                case PlayerState.State.recover:
                    RecoverSize(upSizeSpeed * Time.deltaTime);
                    break;
                case PlayerState.State.toSpace:
                    UpSize(_spaceSize, upSizeSpeed / 4f * Time.deltaTime);
                    break;
                default:
                    break;
            }
        }
    }

    void Shake()
    {
        PlayerState.State statebefore = _playerState._state;
        if (elapsedShakeTime < shakeDuration)
        {
            transform.position = _followPosition + _player.transform.position + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedShakeTime += Time.deltaTime;
        }
        else
        {
            _playerState.SetState(statebefore);
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
        _defaultSize = targetSize;
        RecoverSize(delta);
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
            _playerState.SetState(PlayerState.State.follow);
        }
    }
    #endregion

    #region PublicMethods

    public void HitCameraEffect()
    {
        _playerState.SetState(PlayerState.State.shake);
        elapsedShakeTime = 0;
    }
    #endregion
}
