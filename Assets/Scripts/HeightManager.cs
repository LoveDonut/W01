using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class HeightManager : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] GameObject _oxygenTank;
    [SerializeField] Transform _sunTransform;
    [SerializeField] Transform _skyIslandTransform;
    [SerializeField] float _damageByTimeInSpace = 2f;

    [HideInInspector] public float _sunHeight;
    [HideInInspector] public float _skyIslandHeight;
    [HideInInspector] public float _spaceHeight;

    public float _cometSpawnY = 800f;
    public bool _enteringSpace = false;
    public bool _inSpace = false;


    PlayerController _playerController;
    PlayerState _playerState;

    float _damageDelta;

    #endregion

    #region PrivateMethods
    void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerState = FindObjectOfType<PlayerState>();
    }

    void Update()
    {
        if(_inSpace && !_oxygenTank.activeInHierarchy)
        {
            _playerController.Damage(_damageDelta * Time.deltaTime);
        }
    }

    #endregion

    #region PublicMethods
    public void EnterSpace()
    {
        _playerState.SetState(PlayerState.State.toSpace);
        _damageDelta = _damageByTimeInSpace;
    }

    public void SetSunHeight(float y)
    {
        _sunHeight = y;
    }

    public void SetSkyIslandHeight(float y)
    {
        _skyIslandHeight = y;
        _spaceHeight = y + 100f;
    }
    #endregion
}
