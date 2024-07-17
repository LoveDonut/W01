using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightManager : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] float _skyHeight = 300f;
    [SerializeField] float _spaceHeight = 500f;
    [SerializeField] float backPowerInSpace = 10f;

    PlayerController _playerController;
    PlayerState _playerState;

    bool isStageChanged = false;

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
        if(_playerState.transform.position.y > _spaceHeight && !isStageChanged)
        {
            EnterSpace();
            isStageChanged = true;
        }
    }

    void EnterSpace()
    {
        _playerController.ReducePlayerXSpeed(backPowerInSpace);
        _playerState.SetState(PlayerState.State.toSpace);
        Debug.Log("Enter Space");
    }

    #endregion

    #region PublicMethods
    #endregion

}
