using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class Water : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] ParticleSystem _waterSplash;
    [SerializeField] List<SpriteRenderer> _playerSpriteList;
    PlayerState _playerState;

    #endregion

    #region PublicVariables
    #endregion

    #region PrivateMethods

    void Awake()
    {
        _playerState = FindObjectOfType<PlayerState>();    
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerState.GameOver();
            ParticleSystem instance = Instantiate(_waterSplash, collision.transform.position, _waterSplash.transform.rotation);
            Destroy(instance, _waterSplash.main.duration + _waterSplash.main.startLifetime.constantMax);
            foreach (SpriteRenderer playerSprite in _playerSpriteList)
            {
                playerSprite.color -= new Color(0f,0f,0f, 0.5f);
            }
        }
    }

    #endregion

    #region PublicMethods
    #endregion

}
