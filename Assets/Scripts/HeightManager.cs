using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class HeightManager : MonoBehaviour
{
    #region PrivateVariables


    [SerializeField] Transform _sunTransform;
    [SerializeField] float _damageByTimeInSpace = 2f;

    public float _sunHeight;
    public float _skyIslandHeight = 250f;
    public float _spaceHeight = 400f;
    public float _backPowerInSpace = 0.5f;


    PlayerController _playerController;
    PlayerState _playerState;

    float _damageDelta;
    bool _isStageChanged = false;

    #endregion

    #region PublicVariables
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerState = FindObjectOfType<PlayerState>();
    }

    void Start()
    {
        _sunHeight = _sunTransform.position.y;
    }

    void Update()
    {
        if(_playerState.transform.position.y > _spaceHeight)
        {
            if(!_isStageChanged)
            {
                EnterSpace();
            }
            _playerController.Damage(_damageDelta * Time.deltaTime);
        }
    }

    void EnterSpace()
    {
        _isStageChanged = true;
        _playerState.SetState(PlayerState.State.toSpace);
        _damageDelta = _damageByTimeInSpace;
    }

    #endregion

    #region PublicMethods
    #endregion

}
