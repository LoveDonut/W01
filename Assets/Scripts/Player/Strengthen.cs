using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class Strengthen : MonoBehaviour
{
    #region PrivateVariables

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
        _playerController._jumpDirection += _jumpPowerUp;
        _playerController.feather -= 1;
    }

    public void StrengthenMaxHp()
    {
        _playerController.maxHP += _maxHpUp;
        _playerController.hp = _playerController.maxHP;
        _playerController.feather -= 1;
    }

    #endregion

}
