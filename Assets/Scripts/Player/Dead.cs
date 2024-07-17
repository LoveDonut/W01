using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (collision.CompareTag("Water"))
        {
            _playerState.GameOver();

            FallInWaterEffect(collision);
        }
    }

    private void FallInWaterEffect(Collider2D collision)
    {
        ParticleSystem instance = Instantiate(_waterSplash, collision.transform.position, _waterSplash.transform.rotation);
        Destroy(instance, _waterSplash.main.duration + _waterSplash.main.startLifetime.constantMax);
        foreach (SpriteRenderer playerSprite in _playerSpriteList)
        {
            playerSprite.color -= new Color(0f, 0f, 0f, 0.5f);
        }
    }

    #endregion

    #region PublicMethods
    #endregion
}
