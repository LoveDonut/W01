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

    public float maxHP = 100;
    public int feather = 0;
    public bool _didJump;
    public bool flyTutorial = true;
    public bool jumpTutorial = true;
    public bool holdTutorial = true;
    public bool useStamina = false;
    bool _canFly = true;
    bool holdStatus = false;
    bool holdKeyStatus = false;
    bool holdCoolStatus = false;
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
        if (!_didJump || holdStatus) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            holdTutorial = false;
            _playerAnimator.WingGlide();
            _holdVelocity = _myRigidbody.velocity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {

            if (!holdKeyStatus)
            {
                holdKeyStatus = true;
                StartCoroutine(CheckHoldKey());
            }
            _playerAnimator.WingGlide();
            if (_myRigidbody.velocity.y > 0f)
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdVelocity.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdDownSpeed);
            }
            if (!holdStatus)
            {
                Damage(Time.deltaTime * _holdCost);
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            if(!holdCoolStatus){
            holdStatus = true;
            holdKeyStatus = false;
            StartCoroutine(HoldCoolDown());}else{
                holdStatus = false;
                holdKeyStatus = false;
                holdCoolStatus = false;
                useStamina = false;
            }
        }
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
        Debug.Log(PlayerState._state);
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

    IEnumerator HoldCoolDown()
    {
        holdCoolStatus = true;
        holdStatus = true;
        Debug.Log("쿨다운 시작");
        useStamina = false;
        yield return new WaitForSeconds(1f);
        if (holdKeyStatus)
        {
            useStamina = true;
        }
        holdCoolStatus = false;
        holdStatus = false;
        holdKeyStatus = false;
    }
    IEnumerator CheckHoldKey()
    {
        useStamina = true;
        yield return new WaitForSeconds(3);
        useStamina = false;

        if (holdKeyStatus)
        {
            StartCoroutine(HoldCoolDown());
        }
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
