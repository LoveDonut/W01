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
    public Vector2 _jumpDirection = new Vector2(20,40);
    [SerializeField] float _minPower = 1f;
    [SerializeField] float _maxPower = 3;
    [SerializeField] float _backOffset = -6f;
    [SerializeField] float _backSpeed = 5f;
    [SerializeField] float _goSpeed = 20f;

    [Header("Fly")]
    [SerializeField] Vector2 _flyPower;
    [SerializeField] float _flyCost = 10f;
    [SerializeField] ParticleSystem _flyEffect;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;
    [SerializeField] float _holdCost = 25f;

    [Header("WindPower")]
    [SerializeField] Vector2 windPower;

    Rigidbody2D _myRigidbody;
    FollowCamera _followCamera;
    PlayerState _playerState;
    PlayerAnimator _playerAnimator;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;
    Vector2 tempVector;

    public float maxHP = 120;
    public int feather = 0;
    bool _didJump;
    bool _canFly = true;
    float _startTime, _endTime;
    #endregion

    #region PublicVariables
    public bool IsAlive { get { return hp > 0; } set { } }
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
    }

    void Start()
    {
        hp = maxHP;    
    }

    void Update()
    {
        if (!IsGameStart) return;
        if(PlayerState._state == PlayerState.State.water)
        {
            _myRigidbody.velocity = new Vector2(1f, -3f);            
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

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            _playerAnimator.WingGlide();
            _holdVelocity = _myRigidbody.velocity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
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
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if(transform.position.x > _jumpPosition.x + _backOffset)
            {
                transform.position -= new Vector3(_backSpeed * Time.deltaTime, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _playerAnimator.BodyRun();
            _endTime = Time.time;
            float elapsedTime = Mathf.Clamp(_endTime - _startTime, _minPower, _maxPower);
            StartCoroutine(GoJump(elapsedTime));
            _playerState.SetState(PlayerState.State.recover);
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
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += _flyPower;
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x + _flyPower.x, _flyPower.y);
            }
            hp -= _flyCost;
            _canFly = false;
            _playerState.SetState(PlayerState.State.dash);
            StartCoroutine(FlyCoolDown());
        }
        _playerAnimator.WingFly();
    }

    IEnumerator FlyCoolDown()
    {
        float flyCoolDown = _flyEffect.main.duration + _flyEffect.main.startLifetime.constantMax;
        yield return new WaitForSeconds(flyCoolDown);
        _canFly = true;
        _playerState.SetState(PlayerState.State.recover);
    }

    IEnumerator GoJump(float elapsedTime)
    {
        while (transform.position.x < _jumpPosition.x)
        {
            transform.position += new Vector3(_goSpeed * Time.deltaTime, 0);

            yield return new WaitForEndOfFrame();
        }

        _myRigidbody.AddForce(elapsedTime * _jumpDirection, ForceMode2D.Impulse);
        _didJump = true;

    }
    #endregion

    #region PublicMethods
    public void Damage(float damage)
    {
        hp -= damage;
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
        _myRigidbody.velocity = new Vector2(tempVector.x-0.5f, tempVector.y-1);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Feather")){
            feather++;
        }

        if(other.gameObject.CompareTag("Wind")){
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
            hp -= 10;
            if(hp<=0){
                //gameover
                hp = 0;
            }
        }

        if(other.gameObject.CompareTag("Comet")){
            heightDown();
        }
    }
}
