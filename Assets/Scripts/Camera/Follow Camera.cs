using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

// By Daehee
public class FollowCamera : MonoBehaviour
{
    public enum CameraState
    {
        notStart,
        moveToSun,
        moveToPlayer,
        follow
    };

    #region PrivateVariables
    [Header("When Ground")]
    [SerializeField] float _backSize = 8f;
    [SerializeField] float _defaultSize = 10f;
    [SerializeField] float _downSizeSpeed = -2.5f;

    [Header("When Space")]
    [SerializeField] float _spaceSize = 25f;
    [SerializeField] float _sizeReductionInSpace = 5f;
    [SerializeField] float _downSizeSpeedInSpace = -10f;

    [Header("When Dash")]
    [SerializeField] float _downSizeSpeedWhenDash = -5f;

    [Header("When Look up Sun / SkyIsland")]
    [SerializeField] Vector3 _sunBelowPosition = new Vector3(-25f, -30f);
    [SerializeField] Vector3 _skyIslandUpPosition = new Vector3(0, 10f);
    [SerializeField] Transform _skyIslandTransform;
    [SerializeField] float _startDelay = 0.5f;
    [SerializeField] float _lookUpSunDuration = 2f;

    [Header("When Big Comet Event")]
    [SerializeField] float _lookUpDurationWhenBigCometEvent = 2f;
    [SerializeField] float _sizeWhenCometEvent = 100f;

    [Header("Others")]
    [SerializeField] float _cameraMoveSpeed = 0.7f;

    Camera _camera;
    PlayerController _player;
    PlayerState _playerState;
    HeightManager _heightManager;
    Vector3 _followPosition, _followDashPosition;

    float sizeReductionWhenDash;
    float upSizeSpeed;
    float shakeDuration = 1f;
    float shakeMagnitude = 0.2f;
    float elapsedShakeTime;
    #endregion

    #region PublicVariables

    public Transform _sunTransform;
    public CameraState cameraState = CameraState.notStart;

    #endregion

    #region PrivateMethods
    void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerController>();
        _playerState = FindObjectOfType<PlayerState>();
        _heightManager = FindObjectOfType<HeightManager>();
    }

    void Start()
    {
        sizeReductionWhenDash = 1f;
        _followPosition = new Vector3(7f, 0f, -10f);
        upSizeSpeed = _downSizeSpeed * -2f;
    }

    void LateUpdate()
    {
        if (PlayerState._state == PlayerState.State.water || PlayerState._state == PlayerState.State.clear) return;
        MoveCamera();
    }

    private void MoveCamera()
    {
        if(StrengthenData.instance.isRestart && cameraState == CameraState.notStart)
        {
            cameraState = CameraState.follow;
            _playerState.SetState(PlayerState.State.follow);
        }

        if (cameraState == CameraState.notStart)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(CameraStateChangeDelay(_startDelay, CameraState.moveToSun));
            }
        }
        else if (cameraState == CameraState.moveToSun)
        {
            Vector3 followPosition = !_heightManager._enteringSpace ? 
                     _skyIslandTransform.position + _skyIslandUpPosition : _sunBelowPosition + _sunTransform.position;

            if (isCameraNear(followPosition))
            {
                StartCoroutine(CameraStateChangeDelay(_lookUpSunDuration, CameraState.moveToPlayer));
            }
            else
            {
                MoveTo(followPosition);
            }
        }
        else if (cameraState == CameraState.moveToPlayer)
        {
            if (isCameraNear(_player.transform.position + _followPosition) && PlayerState._state != PlayerState.State.clear)
            {
                cameraState = CameraState.follow;
                _playerState.SetState(PlayerState.State.follow);
            }
            else
            {
                MoveTo(_player.transform.position + _followPosition);
            }
        }
        else
        {
            transform.position = _player.transform.position + _followPosition;
            _player.IsGameStart = true;
            MoveCameraAfterLookUpSun();
        }
    }

    IEnumerator CameraStateChangeDelay(float duration, CameraState state)
    {
        yield return new WaitForSeconds(duration);
        cameraState = state;
    }

    void MoveTo(Vector3 objective)
    {
        Vector2 directionTo = (objective - transform.position);
        transform.position += (Vector3)directionTo * _cameraMoveSpeed;
    }

    bool isCameraNear(Vector3 objective)
    {
        if(Mathf.Abs(transform.position.x - objective.x) + Mathf.Abs(transform.position.y - objective.y) < 0.5f)
        {
            return true;
        }
        return false;
    }

    void MoveCameraAfterLookUpSun()
    {
        if (PlayerState._state == PlayerState.State.shake)
        {
            Shake();
        }
        else
        {
            switch (PlayerState._state)
            {
                case PlayerState.State.back:
                    DownSize(_backSize, _downSizeSpeed * Time.deltaTime);
                    break;
                case PlayerState.State.dash:
                    if (_heightManager._enteringSpace) break;
                    DownSize(_defaultSize - sizeReductionWhenDash, _downSizeSpeedWhenDash * Time.deltaTime);
                    break;
                case PlayerState.State.recover:
                    RecoverSize(upSizeSpeed * Time.deltaTime, 0.2f);
                    break;
                case PlayerState.State.toSpace:
                    sizeReductionWhenDash = _sizeReductionInSpace;
                    _downSizeSpeedWhenDash = _downSizeSpeedInSpace;
                    UpSize(_spaceSize, upSizeSpeed * Time.deltaTime, 0.2f);
                    break;
                case PlayerState.State.cometEvent:
                    UpSize(_sizeWhenCometEvent, upSizeSpeed * Time.deltaTime, 1f);
                    break;
                default:
                    break;
            }
        }
    }

    void Shake()
    {
        PlayerState.State statebefore = PlayerState._state;
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

    void UpSize(float targetSize, float delta, float diff)
    {
        _defaultSize = targetSize;
        RecoverSize(delta, diff);
    }

    void RecoverSize(float delta, float diff)
    {
        if(_camera.orthographicSize < _defaultSize)
        {
            _camera.orthographicSize += delta;
        }
        else if(_camera.orthographicSize > _defaultSize)
        {
            _camera.orthographicSize -= delta;
        }

        if(Mathf.Abs(_camera.orthographicSize - _defaultSize) < diff)
        {
            if(_heightManager._inSpace)
            {
                _defaultSize = _spaceSize;
            }
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

    public void SetDefualtSize(float size)
    {
        _defaultSize = size;
    }

    #endregion
}
