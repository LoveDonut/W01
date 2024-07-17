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
    [SerializeField] Vector2 _jumpDirection = new Vector2(20,40);
    [SerializeField] float _minPower = 1f;
    [SerializeField] float _maxPower = 3;
    [SerializeField] float _backOffset = -6f;
    [SerializeField] float _backSpeed = 5f;
    [SerializeField] float _goSpeed = 20f;

    [Header("Fly")]
    [SerializeField] Vector2 _flyPower = new Vector2(5f,40f);
    [SerializeField] float _flyCost = 10f;
    [SerializeField] ParticleSystem _flyEffect;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;
    [SerializeField] float _holdCost = 25f;

    [Header("Damage")]
    [SerializeField] float _damageByTime = 2f;

    Rigidbody2D _myRigidbody;
    FollowCamera _followCamera;
    PlayerState _playerState;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;
    Vector2 tempVector;

    public int maxHP = 120;
    public int feather = 0;
    bool _didJump;
    bool _canFly = true;
    float _startTime, _endTime;
    #endregion

    #region PublicVariables
    public bool IsAlive { get { return hp > 0; } set { } }
    public bool _isGameStart;
    public float hp;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _followCamera = FindObjectOfType<FollowCamera>();
        _playerState = GetComponent<PlayerState>();
    }

    void Start()
    {
        hp = maxHP;    
    }

    void Update()
    {
        if (!IsAlive) return;
        JumpStart();
        Fly();
        Hold();
        Damage(Time.deltaTime * _damageByTime);

//        Debug.Log(_myRigidbody.velocity);
    }

    void Hold()
    {
        if (!_didJump) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            _holdVelocity = _myRigidbody.velocity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
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
            _endTime = Time.time;
            float elapsedTime = Mathf.Clamp(_endTime - _startTime, _minPower, _maxPower);
            StartCoroutine(GoJump(elapsedTime));
            _playerState.SetState(PlayerState.State.recover);
        }
    }

    void Fly()
    {
        if(hp < _flyCost || !_didJump || !_canFly) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
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
//        _myRigidbody.AddForce(new Vector2(power, 0f), ForceMode2D.Impulse);
        _myRigidbody.velocity -= new Vector2(_myRigidbody.velocity.x / 2f, 0);
        Debug.Log("after - "+ _myRigidbody.velocity.x);
    }
    #endregion

    void heightDown(){
        tempVector = _myRigidbody.velocity;
        _myRigidbody.gravityScale = 0;
        _myRigidbody.velocity = new Vector2(0, -7f);
        StartCoroutine(wait2Seconds());
    }

    IEnumerator wait2Seconds(){
        yield return new WaitForSeconds(1.0f);
        _myRigidbody.velocity = tempVector;
        _myRigidbody.gravityScale = 3;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Feather")){
            feather++;
        }

        if(other.gameObject.CompareTag("Wind")){
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += new Vector2(0f, _flyPower.y * 2f);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, _flyPower.y * 2f);
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
