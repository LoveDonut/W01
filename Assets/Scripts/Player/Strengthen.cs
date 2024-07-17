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
        StrengthenData.instance.jumpDirection += _jumpPowerUp;
        StrengthenData.instance.feather--;
    }

    public void StrengthenMaxHp()
    {
        StrengthenData.instance.maxHp += _maxHpUp;
        StrengthenData.instance.feather--;
    }

    #endregion

}
