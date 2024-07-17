using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Daehee
public class FollowCamera : MonoBehaviour
{

    #region PrivateVariables
    [SerializeField] float _backSize = 5f;
    [SerializeField] float _defaultSize = 7f;
    [SerializeField] float _spaceSize = 10f;
    [SerializeField] float _downSizeSpeed = -2.5f;

    Camera _camera;
    Transform _player;
    PlayerState _playerState;
    Vector3 _followPosition, _followDashPosition;
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
        _player = FindObjectOfType<PlayerController>().transform;
        _playerState = FindObjectOfType<PlayerState>();
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
        if (_playerState._state == PlayerState.State.shake)
        {
            Shake();
        }
        else
        {
            transform.position = _player.position + _followPosition;
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
            transform.position = _followPosition + _player.position + (Vector3)Random.insideUnitCircle * shakeMagnitude;
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
