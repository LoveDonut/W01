using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightManager : MonoBehaviour
{
    #region PrivateVariables

    PlayerController _playerController;
    PlayerState _playerState;

    [SerializeField] float _skyHeight = 300f;
    [SerializeField] float _spaceHeight = 500f;
    [SerializeField] float backPowerInSpace = 10f;

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
        if(_playerState.transform.position.y > _spaceHeight)
        {
            EnterSpace();
        }
    }

    void EnterSpace()
    {
        _playerController.ReducePlayerXSpeed(backPowerInSpace);
        _playerState.SetState(PlayerState.State.toSpace);
    }

    #endregion

    #region PublicMethods
    #endregion

}
