using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// by Daehee
public class PlayerControllerCopy : MonoBehaviour
{
    #region PrivateVariables
    [Header("Jump")]
    public Vector2 _jumpDirection = new Vector2(20,40);
    [SerializeField] float _minPower = 1f;
    [SerializeField] float _maxPower = 3;
    [SerializeField] float backOffset = -6f;
    [SerializeField] float backSpeed = 5f;
    [SerializeField] float goSpeed = 20f;

    [Header("Fly")]
    [SerializeField] Vector2 _flyDirection = new Vector2(1,40);
    [SerializeField] float flyCost = 10f;

    [Header("Hold")]
    [SerializeField] float _holdDownSpeed = -5f;
    [SerializeField] float holdCost = 25f;

    [Header("Damage")]
    [SerializeField] float damageByTime = 2f;

    Rigidbody2D _myRigidbody;
    FollowCameraCopy _followCamera;

    Vector2 _holdVelocity;
    Vector2 _jumpPosition;

    public Text featherText;
    public Text hpText;

    [Header("UI")]
    public int maxHP = 120;
    public int feather = 0;
    bool _isStart;
    float _startTime, _endTime;

    [Header("Animation")]
    public Animator BodyAnim;
    public Animator WingAnim;
    #endregion

    #region PublicVariables
    public bool IsAlive { get { return hp > 0; } set { } }
    public float hp = 100f;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _followCamera = FindObjectOfType<FollowCameraCopy>();
    }

    void Update()
    {
        if (!IsAlive) return;
        JumpStart();
        Fly();
        Hold();
        Damage(Time.deltaTime * damageByTime);
    }

    void Hold()
    {
        if (!_isStart) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            //playerAnimator.WingGlide() method call
            _holdVelocity = _myRigidbody.velocity;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) 
        {
            //playerAnimator.WingGlide() method call
            if (_myRigidbody.velocity.y > 0f)
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdVelocity.y);
            }
            else
            {
                _myRigidbody.velocity = new Vector2(_holdVelocity.x, _holdDownSpeed);
            }
            Damage(Time.deltaTime * holdCost);
        }
    }

    void JumpStart()
    {
        if (_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerAnimator.BodyBack() method call
            BodyAnim.SetTrigger("_readyToJump");
            _startTime = Time.time;
            _jumpPosition = transform.position;
            _followCamera.SetState(FollowCameraCopy.State.back);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (transform.position.x > _jumpPosition.x + backOffset)
            {
                transform.position -= new Vector3(backSpeed * Time.deltaTime, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //playerAnimator.BodyRun() method call
            BodyAnim.SetTrigger("_running");
            _endTime = Time.time;
            float elapsedTime = Mathf.Clamp(_endTime - _startTime, _minPower, _maxPower);
            Debug.Log(elapsedTime);
            StartCoroutine(GoJump(elapsedTime));
            _followCamera.SetState(FollowCameraCopy.State.recover);

            //playerAnimator.BodyFly() && playerAnimator.WingJump() methods call
            BodyAnim.SetTrigger("_flying");
            WingAnim.SetTrigger("_jump");
        }
    }

    void Fly()
    {
        if(hp < flyCost || !_isStart) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //playerAnimator.WingJump() method call
            WingAnim.ResetTrigger("_fly");
            WingAnim.ResetTrigger("_glide");
            WingAnim.SetTrigger("_jump");
            _myRigidbody.AddForce(_flyDirection, ForceMode2D.Impulse);
            hp -= flyCost;
        }

        //playerAnimator.WingFly() method call
        WingAnim.SetTrigger("_fly");
    }

    IEnumerator GoJump(float elapsedTime)
    {
        while (transform.position.x < _jumpPosition.x)
        {
            transform.position += new Vector3(goSpeed * Time.deltaTime, 0);

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

        Vector2 tempVector = _myRigidbody.velocity;
        _myRigidbody.gravityScale = 0;
        _myRigidbody.velocity = new Vector2(0, -7f);

        StartCoroutine(wait2Seconds());

        _myRigidbody.velocity = tempVector;
        _myRigidbody.gravityScale = 3;
    }

    IEnumerator wait2Seconds(){

        yield return new WaitForSeconds(5.0f);

    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Feather")){
            feather++;
            featherText.text = "Feather "+feather;
        }

        if(other.gameObject.CompareTag("Wind")){
            _myRigidbody.AddForce(_flyDirection, ForceMode2D.Impulse);
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
