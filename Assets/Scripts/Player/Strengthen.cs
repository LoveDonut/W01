using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class Strengthen : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] GameObject _shield;
    [SerializeField] GameObject _oxygenTank;
    public Vector2 _jumpPowerUp = new Vector2(5f,5f);
    public float _maxHpUp = 10f;

    PlayerController _playerController;

    #endregion

    #region PublicVariables
    #endregion

    #region PrivateMethods

    void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();    
    }

    #endregion

    #region PublicMethods

    public void StrengthenJumpPower()
    {
        if (_playerController.feather <= 0) return;
        _playerController._jumpDirection += _jumpPowerUp;
        _playerController.feather -= 1;
    }

    public void StrengthenMaxHp()
    {
        if (_playerController.feather <= 0) return;
        _playerController.maxHP += _maxHpUp;
        _playerController.hp = _playerController.maxHP;
        _playerController.feather -= 1;
    }

    public void SetShield()
    {
        if (_playerController.feather < 5) return;
        _playerController.feather -= 5;
        _shield.SetActive(true);
    }

    public void SetOxygenTank()
    {
        if (_playerController.feather < 5) return;
        _playerController.feather -= 5;
        _oxygenTank.SetActive(true);
    }

    #endregion

}
