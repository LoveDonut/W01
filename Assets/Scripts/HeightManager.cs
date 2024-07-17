using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightManager : MonoBehaviour
{
    #region PrivateVariables



    [SerializeField] float _damageByTimeInSpace = 2f;

    public float _skyHeight;
    public float _spaceHeight;
    public float _sunHeight;
    public float _cometHeight;
    public float backPowerInSpace = 10f;


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

    }

    void Update()
    {
        if(_playerState.transform.position.y > _spaceHeight && !_isStageChanged)
        {
            EnterSpace();
        }
        _playerController.Damage(_damageDelta * Time.deltaTime);
    }

    void EnterSpace()
    {
        _playerController.ReducePlayerXSpeed(_backPowerInSpace);
        _playerState.SetState(PlayerState.State.toSpace);
        _isStageChanged = true;
        _damageDelta = _damageByTimeInSpace;
    }

    #endregion

    #region PublicMethods
    #endregion

}
