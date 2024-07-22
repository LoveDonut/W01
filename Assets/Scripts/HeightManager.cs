using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class HeightManager : MonoBehaviour
{
    #region PrivateVariables


    [SerializeField] Transform _sunTransform;
    [SerializeField] Transform _skyIslandTransform;
    [SerializeField] float _damageByTimeInSpace = 2f;

    [HideInInspector] public float _sunHeight;
    [HideInInspector] public float _skyIslandHeight;
    [HideInInspector] public float _spaceHeight;
    
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

    void Start()
    {
        _skyIslandHeight = _skyIslandTransform.position.y;
        _spaceHeight = _skyIslandTransform.position.y + 100f;
    }

    void Update()
    {
        if(_inSpace)
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
    #endregion
}
