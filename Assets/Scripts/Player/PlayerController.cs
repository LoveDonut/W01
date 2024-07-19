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
    [SerializeField] float _jumpCostMultiply = 1.5f;

    [Header("Fly")]
    [SerializeField] Vector2 _flyPower = new Vector2(1.5f, 25f);
    [SerializeField] float _flyCost = 10f;
    [SerializeField] ParticleSystem _flyEffect;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;
    [SerializeField] float _holdCost = 2f;

    [Header("Wind")]
    [SerializeField] Vector2 windPower;
    [SerializeField] ParticleSystem windEffect;

    [Header("Others")]
    [SerializeField] Transform _sunTransform;
    [SerializeField] float speedMoveToSunAfterClear = 10f;

    Rigidbody2D _myRigidbody;
    FollowCamera _followCamera;
    PlayerState _playerState;
    PlayerAnimator _playerAnimator;
    GameClear _gameClear;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;
    Vector2 tempVector;

    private enum ShiftState
    {
        Idle,           // 기본 상태
        Pressed,        // SHIFT 키를 누른 상태
        Delayed,        // 딜레이 상태
    }
    private ShiftState currentState = ShiftState.Idle;
    private float shiftHoldTime = 0f;   // SHIFT 키를 누른 시간
    private const float maxShiftHoldTime = 3f;  // 최대 SHIFT 키 누르는 시간 (3초)
    private const float delayTime = 1f;  // 딜레이 시간 (1초)
    private bool delayActive = false;    // 딜레이가 활성화되었는지 여부를 나타내는 플래그


    public float maxHP = 100;
    public int feather = 0;
    public bool _didJump;
    public bool flyTutorial = true;
    public bool jumpTutorial = true;
    public bool holdTutorial = true;
    public bool useStamina = false;
    bool _canFly = true;
    bool _isSpaceKeyDown = false;

    float _startTime, _endTime;
    #endregion

    #region PublicVariables
    public bool IsAlive = true;
    // { get { return hp > 0; } set { } }
    public bool IsGameStart = false;
    // { get { return _playerState._state != PlayerState.State.LookupSun; } }
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
    }

    void Start()
    {
        if(StrengthenData.instance != null)
        {
            maxHP += StrengthenData.instance.maxHpUp;
            _jumpDirection += StrengthenData.instance.jumpPowerUp;
            feather = StrengthenData.instance.feather;
        }
        hp = maxHP;
    }

    void Update()
    {

        if (!IsGameStart) return;
        if (PlayerState._state == PlayerState.State.water)
        {
            _myRigidbody.velocity = new Vector2(1f, -3f);
        }
        else if (PlayerState._state == PlayerState.State.clear)
        {
//            Debug.Log(PlayerState._state);
            Vector3 directionToSun = (_sunTransform.position - transform.position).normalized;
            _myRigidbody.velocity = directionToSun * speedMoveToSunAfterClear;
        }
        else
        {
            JumpStart();
            Fly();
            Hold();
        }
    }

    void Hold()
    {
        if (!_didJump) return;
        switch (currentState)
        {
            case ShiftState.Idle:
                _holdVelocity = _myRigidbody.velocity;
                HandleIdleState();
                break;
            case ShiftState.Pressed:
                HandlePressedState();
                break;
        }
    }

    void HandleIdleState()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            holdTutorial = false;
            currentState = ShiftState.Pressed;
            shiftHoldTime = 0f;
        }else if(Input.GetKey(KeyCode.LeftShift)){
            currentState = ShiftState.Pressed;
        }
    }

    void HandlePressedState()
    {
        shiftHoldTime += Time.deltaTime;
        
        if(!delayActive){
            _playerAnimator.WingGlide();
            if (_myRigidbody.velocity.y > 0f)
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdVelocity.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdDownSpeed);
            }
            Damage(Time.deltaTime * _holdCost);
            useStamina = true;
            if (shiftHoldTime >= maxShiftHoldTime)
            {
                currentState = ShiftState.Delayed;
                StartCoroutine(DelayCoroutine());
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // SHIFT 키를 뗀 경우
            if (!delayActive && shiftHoldTime < maxShiftHoldTime)
            {
                // 최대 시간 미만으로 누르고 떼면 바로 딜레이 상태로 전환
                currentState = ShiftState.Delayed;
                useStamina = false;
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                // 딜레이가 활성화된 상태에서 떼면 기본 상태로 전환
                currentState = ShiftState.Idle;
            }
        }
    }

    IEnumerator DelayCoroutine()
    {
        delayActive = true;
        useStamina = false;
        yield return new WaitForSeconds(delayTime);
        delayActive = false;
        shiftHoldTime = 0f;
        currentState = ShiftState.Idle;
    }

    void JumpStart()
    {
        if (_didJump) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hihi");
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
            int jumpCost = Mathf.RoundToInt(-(transform.position.x + 6) * _jumpCostMultiply);

            StartCoroutine(GoJump(elapsedTime, jumpCost));
            _playerState.SetState(PlayerState.State.recover);
            _playerAnimator.BodyFly();
            _playerAnimator.WingJump();
            Debug.Log(jumpCost);
        }
    }

    void Fly()
    {
        //Debug.Log(PlayerState._state);
        if(hp < _flyCost || !_didJump || !_canFly) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerAnimator.WingJumpReset();
            _flyEffect.Play();
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += _flyPower;
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x + _flyPower.x, _flyPower.y);
            }
            Damage(_flyCost);
            _canFly = false;
            flyTutorial = false;
            if (PlayerState._state != PlayerState.State.toSpace)
            {
                _playerState.SetState(PlayerState.State.dash);
            }
            StartCoroutine(FlyCoolDown());
        }
        _playerAnimator.WingFly();
    }

    IEnumerator FlyCoolDown()
    {
        float flyCoolDown = _flyEffect.main.duration + _flyEffect.main.startLifetime.constantMax;
        yield return new WaitForSeconds(flyCoolDown);
        _canFly = true;
        if (PlayerState._state != PlayerState.State.toSpace)
        {
            _playerState.SetState(PlayerState.State.recover);
        }
    }

    IEnumerator GoJump(float elapsedTime, int jumpCost)
    {
        while (transform.position.x < _jumpPosition.x)
        {
            transform.position += new Vector3(_goSpeed * Time.deltaTime, 0);

            yield return new WaitForEndOfFrame();
        }

        _myRigidbody.AddForce(elapsedTime * _jumpDirection, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.3f);
        _didJump = true;
        jumpTutorial = false;
        Damage(jumpCost);
    }

    #endregion

    #region PublicMethods
    public void Damage(float damage)
    {
        hp = Mathf.Clamp(hp - damage, 0f, maxHP);
    }

    public void ReducePlayerXSpeed(float power)
    {
        Debug.Log("before - " + _myRigidbody.velocity.x);
        _myRigidbody.velocity -= new Vector2(_myRigidbody.velocity.x / 2f, 0);
        Debug.Log("after - "+ _myRigidbody.velocity.x);
    }
    #endregion

    void heightDown(){
        tempVector = _myRigidbody.velocity;
        _myRigidbody.velocity = new Vector2(tempVector.x-3f, tempVector.y-12f);
    }

    void OnTriggerEnter2D(Collider2D other){
        if (hp <= 0) return;
        if(other.gameObject.CompareTag("Feather")){
            feather++;
        }

        if(other.gameObject.CompareTag("Wind")){
            windEffect.Play();
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += windPower;
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, windPower.y);
            }
        }

        if (other.gameObject.CompareTag("HPup")){
            hp += 10;
            if(hp >= maxHP){
                hp = maxHP;
            }
        }

        if(other.gameObject.CompareTag("HPdown")){
            Damage(10);
            if(hp<=0){
                //gameover
                hp = 0;
            }
        }

        if(other.gameObject.CompareTag("Comet")){
            heightDown();
        }

        if (other.gameObject.CompareTag("Sun"))
        {
            _gameClear.EnterSun();

        }
    }
}
