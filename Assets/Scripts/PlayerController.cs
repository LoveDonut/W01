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
    [SerializeField] float _flyPower = 40f;
    [SerializeField] float _flyCost = 10f;
    [SerializeField] ParticleSystem _flyEffect;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;
    [SerializeField] float _holdCost = 25f;

    [Header("Damage")]
    [SerializeField] float _damageByTime = 2f;

    Rigidbody2D _myRigidbody;
    FollowCamera _followCamera;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;

    public Text featherText;
    public Text hpText;

    public int maxHP = 120;
    public int feather = 0;
    bool _isStart;
    bool _canFly = true;
    float _startTime, _endTime;
    #endregion

    #region PublicVariables
    public bool IsAlive { get { return hp > 0; } set { } }
    public float hp = 100f;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _followCamera = FindObjectOfType<FollowCamera>();
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
        if (!_isStart) return;

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
        if (_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _startTime = Time.time;
            _jumpPosition = transform.position;
            _followCamera.SetState(FollowCamera.State.back);
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
            _followCamera.SetState(FollowCamera.State.recover);
        }
    }

    void Fly()
    {
        if(hp < _flyCost || !_isStart || !_canFly) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _flyEffect.Play();
            if (_myRigidbody.velocity.y > 0)
            {
                _myRigidbody.velocity += new Vector2(1f, _flyPower);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, _flyPower);
            }
            hp -= _flyCost;
            _canFly = false;
            _followCamera.SetState(FollowCamera.State.dash);
            StartCoroutine(FlyCoolDown());
        }
    }

    IEnumerator FlyCoolDown()
    {
        float flyCoolDown = _flyEffect.main.duration + _flyEffect.main.startLifetime.constantMax;
        yield return new WaitForSeconds(flyCoolDown);
        _canFly = true;
        _followCamera.SetState(FollowCamera.State.recover);
    }

    IEnumerator GoJump(float elapsedTime)
    {
        while (transform.position.x < _jumpPosition.x)
        {
            transform.position += new Vector3(_goSpeed * Time.deltaTime, 0);

            yield return new WaitForEndOfFrame();
        }

        _myRigidbody.AddForce(elapsedTime * _jumpDirection, ForceMode2D.Impulse);
        _isStart = true;

    }
    #endregion

    #region PublicMethods
    public void Damage(float damage)
    {
        hp -= damage;
    }
    #endregion

    void updateHPText(){
        hpText.text = "HP "+(int)hp;
    }

    void heightDown(){
        Debug.Log("comet");
        Vector2 tempVector = _myRigidbody.velocity;
        _myRigidbody.gravityScale = 0;
        _myRigidbody.velocity = new Vector2(0, -7f);
        Debug.Log("코루틴 시작");
        StartCoroutine(wait2Seconds());
        Debug.Log("코루틴 끝");
        _myRigidbody.velocity = tempVector;
        _myRigidbody.gravityScale = 3;
    }

    IEnumerator wait2Seconds(){
        Debug.Log("코루틴 도달");
        yield return new WaitForSeconds(5.0f);
        Debug.Log("5초 후");
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Feather")){
            feather++;
            featherText.text = "Feather "+feather;
        }

        if(other.gameObject.CompareTag("Wind")){
            _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, _flyPower * 2f);
        }

        if(other.gameObject.CompareTag("HPup")){
            hp += 10;
            if(hp >= maxHP){
                hp = maxHP;
            }
            updateHPText();
        }

        if(other.gameObject.CompareTag("HPdown")){
            hp -= 10;
            if(hp<=0){
                //gameover
                hp = 0;
            }
            updateHPText();
        }

        if(other.gameObject.CompareTag("Comet")){
            heightDown();
        }
    }
}
