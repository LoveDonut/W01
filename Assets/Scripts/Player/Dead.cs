using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] ParticleSystem _waterSplash;
    [SerializeField] List<SpriteRenderer> _playerSpriteList;
    [SerializeField] float _deadFallSpeedIncrase = 100f;
    
    PlayerController _playerController;
    PlayerState _playerState;
    Rigidbody2D _myRigidbody;

    float _deadFallSpeed;

    #endregion

    #region PublicVariables
    #endregion

    #region PrivateMethods

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _playerState = GetComponent<PlayerState>();
        _myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (PlayerState._state != PlayerState.State.water && _playerController.hp <= 0)
        {
            _deadFallSpeed -= _deadFallSpeedIncrase * Time.deltaTime;
            _myRigidbody.velocity = new Vector2(0f, _deadFallSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water") || (collision.CompareTag("SkyIsland")) && _playerController.hp <= 0)
        {
            _playerState.GameOver();

            if(collision.CompareTag("Water"))
            {
                FallInWaterEffect(collision);
            }
            StartCoroutine(Restart());
        }
    }

    void FallInWaterEffect(Collider2D collision)
    {
        ParticleSystem instance = Instantiate(_waterSplash, transform.position, _waterSplash.transform.rotation);
        Destroy(instance, _waterSplash.main.duration + _waterSplash.main.startLifetime.constantMax);
        foreach (SpriteRenderer playerSprite in _playerSpriteList)
        {
            playerSprite.color -= new Color(0f, 0f, 0f, 0.5f);
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        StrengthenData.instance.jumpPowerUp = _playerController._jumpDirection - StrengthenData.instance.defaultJumpPower;
        StrengthenData.instance.maxHpUp = _playerController.maxHP - StrengthenData.instance.defaultHp;
        StrengthenData.instance.feather = _playerController.feather;
        StrengthenData.instance.isRestart = true;
        SceneManager.LoadScene(0);
    }

    #endregion

    #region PublicMethods
    #endregion
}
