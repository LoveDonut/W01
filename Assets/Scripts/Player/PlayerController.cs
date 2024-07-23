using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by Daehee
public class PlayerController : MonoBehaviour
{
    #region PrivateVariables
    [Header("Jump")]
    public Vector2 _jumpDirection = new Vector2(15,25);
    [SerializeField] float _minPower = 1f;
    [SerializeField] float _maxPower = 3f;
    [SerializeField] float _backOffset = -6f;
    [SerializeField] float _backSpeed = 5f;
    [SerializeField] float _goSpeed = 20f;

    [Header("Fly")]
    [SerializeField] Vector2 _flyPower = new Vector2(1.5f, 25f);
    [SerializeField] float _flyCost = 10f;
    [SerializeField] ParticleSystem _flyEffect;
    [SerializeField] float _minX = 2f;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;
    [SerializeField] float _gravityScale = 3f;

    [Header("Wind")]
    [SerializeField] Vector2 windPower;
    [SerializeField] ParticleSystem windEffect;

    [Header("Others")]
    [SerializeField] Transform _sunTransform;
    [SerializeField] float speedMoveToSunAfterClear = 10f;
    [SerializeField] float _maxXSpeed = 30f;
    [SerializeField] GameObject _skyIsland;
    [SerializeField] float _landingYPosition = 256f;
    [SerializeField] GameObject LandingTutorial;


    Rigidbody2D _myRigidbody;
    FollowCamera _followCamera;
    PlayerState _playerState;
    PlayerAnimator _playerAnimator;
    GameClear _gameClear;
    UIController _uiController;
    HeightManager _heightManager;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;
    Vector2 tempVector;

    [HideInInspector] public Vector3 _fixedPosition, _fixedVelocity;
    public float maxHP = 100;
    public int feather = 0;
    public float _holdCoolTime = 1f;
    public bool _didJump;
    public bool flyTutorial = true;
    public bool jumpTutorial = true;
    public bool holdTutorial = true;
    public bool useStamina = false;
    public bool _selectItem = false;
    bool _canFly = true;
    bool _isSpaceKeyDown = false;
    bool _alreadyLand;

    float _startTime, _endTime;
    float _gravityBefore;
    float _direction = 1f;

    #endregion

    #region PublicVariables
    public bool IsAlive = true;
    public bool IsGameStart = false;
    public float hp;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _followCamera = FindObjectOfType<FollowCamera>();
        _playerState = GetComponent<PlayerState>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _gameClear = FindObjectOfType<GameClear>();
        _uiController = FindObjectOfType<UIController>();
        _heightManager = FindObjectOfType<HeightManager>();
    }

    void Start()
    {
        if(StrengthenData.instance != null)
        {
            maxHP += StrengthenData.instance.maxHpUp;
            _jumpDirection += StrengthenData.instance.jumpPowerUp;
            feather = StrengthenData.instance.feather;
            transform.position = StrengthenData.instance._position;
        }
        hp = maxHP;
    }

    PlayerState.State beforeState = PlayerState.State.NotStart;

    void Update()
    {
        if(beforeState != PlayerState._state)
        {
            Debug.Log($"{beforeState} -> {PlayerState._state}");
        }
        beforeState = PlayerState._state;

        if (!IsGameStart || _selectItem) return;
        if (PlayerState._state == PlayerState.State.water)
        {
            _myRigidbody.velocity = new Vector2(1f, -3f);
        }
        else if (PlayerState._state == PlayerState.State.clear)
        {
            Vector3 directionToSun = (_sunTransform.position - transform.position).normalized;
            _myRigidbody.velocity = directionToSun * speedMoveToSunAfterClear;
        }
        else if (PlayerState._state != PlayerState.State.toSpace)
        {
            SetDirection();
            if(_followCamera.cameraState == FollowCamera.CameraState.follow)
            {
                JumpStart();
            }
            Fly();
            Hold();
        }
        LimitMove();
    }

    void LimitMove()
    {
        float skyIslandHalfOfX = _skyIsland.GetComponent<BoxCollider2D>().bounds.size.x * 0.45f;
        _myRigidbody.velocity = new Vector2(Mathf.Clamp(_myRigidbody.velocity.x, -_maxXSpeed, _maxXSpeed), _myRigidbody.velocity.y);

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // when land on SkyIsland
        if (collision.gameObject.CompareTag("SkyIsland") && hp > 0 && !_alreadyLand)
        {
            _alreadyLand = true;
            transform.position = new Vector2(_skyIsland.transform.position.x, _landingYPosition);
            _myRigidbody.velocity = new Vector3(0f, 0f);
            _uiController.TurnOnActiveItemUI();
            _selectItem = true;
            _didJump = false;
            _heightManager._enteringSpace = true;
            LandingTutorial.SetActive(false);
            _playerAnimator.BodyIdle();

            // set direction to front
            _direction = 1f;
        }
    }

    void SetDirection()
    {
        transform.localScale = new Vector3(_direction, 1f, 1f);

        if (!_didJump) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _direction = -1f;
            _flyPower = new Vector2(-Mathf.Abs(_flyPower.x), _flyPower.y);
            windPower = new Vector2(-Mathf.Abs(windPower.x), windPower.y);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _direction = 1f;
            _flyPower = new Vector2(Mathf.Abs(_flyPower.x), _flyPower.y);
            windPower = new Vector2(Mathf.Abs(windPower.x), windPower.y);
        }
    }

    void Hold()
    {
        _holdCoolTime = Mathf.Clamp(_holdCoolTime - Time.deltaTime, 0f, 1f);
        if (!_didJump || _uiController.GetStamina() <= 0f)
        {
            useStamina = false;
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            holdTutorial = false;
            _playerAnimator.WingGlide();
            _holdVelocity = _myRigidbody.velocity;
            _gravityBefore = _myRigidbody.gravityScale;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            useStamina = true;
            _playerAnimator.WingGlide();
            _holdCoolTime = 1f;
            if (_myRigidbody.velocity.y > 0f)
            {
                _myRigidbody.gravityScale = _gravityScale;
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdDownSpeed);
            }
            if (_direction > 0f)
            {
                _myRigidbody.velocity = new Vector2(Mathf.Abs(_myRigidbody.velocity.x), _myRigidbody.velocity.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(-Mathf.Abs(_myRigidbody.velocity.x), _myRigidbody.velocity.y);
            }

        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            useStamina = false;
            _myRigidbody.gravityScale = _gravityBefore;
        }
    }

    void JumpStart()
    {
        if (_didJump) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerAnimator.BodyBack();
            _startTime = Time.time;
            _jumpPosition = transform.position;
            _playerState.SetState(PlayerState.State.back);
            _isSpaceKeyDown = true;
        }
        if (Input.GetKey(KeyCode.Space) && _isSpaceKeyDown)
        {
            if(transform.position.x > _jumpPosition.x + _backOffset)
            {
                transform.position -= new Vector3(_backSpeed * Time.deltaTime, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) && _isSpaceKeyDown)
        {
            _playerAnimator.BodyRun();
            _endTime = Time.time;
            float elapsedTime = Mathf.Clamp(_endTime - _startTime, _minPower, _maxPower);

            StartCoroutine(GoJump(elapsedTime));
            _playerAnimator.BodyFly();
            _playerAnimator.WingJump();
        }
    }

    void Fly()
    {
        if(hp < _flyCost || !_didJump || !_canFly) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerAnimator.WingJumpReset();
            _flyEffect.Play();

            // for x speed
            if(_myRigidbody.velocity.x * _direction < 0)
            {
                _myRigidbody.velocity = new Vector2(-1f * _myRigidbody.velocity.x, _myRigidbody.velocity.y);
            }
            else
            {
                _myRigidbody.velocity += new Vector2(_flyPower.x, 0f);
            }

            // for y speed
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += new Vector2(_myRigidbody.velocity.x, _flyPower.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, _flyPower.y);
            }
            if(-_minX < _myRigidbody.velocity.x && _myRigidbody.velocity.x < _minX)
            {
                if(-_minX < _myRigidbody.velocity.x)
                {
                    _myRigidbody.velocity = new Vector3(-_minX, _myRigidbody.velocity.y);
                }
                else
                {
                    _myRigidbody.velocity = new Vector3(_minX, _myRigidbody.velocity.y);
                }
            }

            Damage(_flyCost);

            _canFly = false;
            flyTutorial = false;
            if (PlayerState._state != PlayerState.State.toSpace && PlayerState._state != PlayerState.State.cometEvent)
            {
                _playerState.SetState(PlayerState.State.dash);
            }
            StartCoroutine(FlyCoolDown());
        }
        _playerAnimator.WingFly();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hp < 0.5f) return;
        if (other.gameObject.CompareTag("Feather"))
        {
            feather++;
            other.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Wind"))
        {
            windEffect.Play();
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += windPower;
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, windPower.y * 1.5f);
            }
            if (-_minX < _myRigidbody.velocity.x && _myRigidbody.velocity.x < _minX)
            {
                if (-_minX < _myRigidbody.velocity.x)
                {
                    _myRigidbody.velocity = new Vector3(-_minX, _myRigidbody.velocity.y);
                }
                else
                {
                    _myRigidbody.velocity = new Vector3(_minX, _myRigidbody.velocity.y);
                }
            }
        }

        if (other.gameObject.CompareTag("HPup"))
        {
            hp += 10;
            other.gameObject.GetComponent<CircleCollider2D>().enabled=false;
            other.gameObject.SetActive(false);
            if (hp >= maxHP)
            {
                hp = maxHP;
            }
        }

        if (other.gameObject.CompareTag("Sun"))
        {
            _gameClear.EnterSun();

        }
    }

    IEnumerator FlyCoolDown()
    {
        float flyCoolDown = _flyEffect.main.duration + _flyEffect.main.startLifetime.constantMax;
        yield return new WaitForSeconds(flyCoolDown);
        _canFly = true;
        if (PlayerState._state != PlayerState.State.toSpace && PlayerState._state != PlayerState.State.clear && PlayerState._state != PlayerState.State.cometEvent)
        {
            _playerState.SetState(PlayerState.State.recover);
        }
    }

    IEnumerator GoJump(float elapsedTime)
    {
        while (transform.position.x < _jumpPosition.x)
        {
            transform.position += new Vector3(_goSpeed * Time.deltaTime, 0);

            yield return new WaitForEndOfFrame();
        }

        if (_heightManager._enteringSpace)
        {
            _heightManager.EnterSpace();
            _heightManager._inSpace = true;
            _skyIsland.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(TurnOffEnteringSpace());
        }

        _myRigidbody.AddForce(elapsedTime * _jumpDirection * new Vector2(_direction, 1f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        _playerState.SetState(PlayerState.State.recover);
        _didJump = true;
        jumpTutorial = false;
    }

    IEnumerator TurnOffEnteringSpace()
    {
        yield return new WaitForSeconds(4f);
        _heightManager._enteringSpace = false;
    }
    #endregion

    #region PublicMethods
    public void Damage(float damage)
    {
        hp = Mathf.Clamp(hp - damage, 0f, maxHP);
    }

    public void ReducePlayerXSpeed(float powerRate)
    {
        _myRigidbody.velocity -= new Vector2(_myRigidbody.velocity.x * powerRate, 0);
    }

    public void MovePlayerToCenterofSkyIsland()
    {
        transform.position = new Vector2(_skyIsland.transform.position.x, transform.position.y);
    }
    #endregion

}
