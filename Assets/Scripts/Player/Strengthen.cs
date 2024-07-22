using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class Strengthen : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] GameObject _shield;
    [SerializeField] GameObject _oxygenTank;
    [SerializeField] GameObject _shieldButton;
    [SerializeField] GameObject _oxygenTankButton;
    public Vector2 _jumpPowerUp = new Vector2(3f,5f);
    public float _maxHpUp = 10f;
    bool _isShieldClicked;
    bool _isTankClicked;


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
        if (_playerController.feather <= 0 || _playerController._jumpDirection.x + _playerController._jumpDirection.y > 100f) return;
        _playerController._jumpDirection += _jumpPowerUp;
        _playerController.feather -= 1;
    }

    public void StrengthenMaxHp()
    {
        if (_playerController.feather <= 0 || _playerController.maxHP >= 200) return;
        _playerController.maxHP += _maxHpUp;
        _playerController.hp = _playerController.maxHP;
        _playerController.feather -= 1;
    }

    public void SetShield()
    {
        if (_playerController.feather < 1) return;
        _isShieldClicked = true;
        if (_isShieldClicked)
        {
            _shieldButton.SetActive(false);
        }
        _playerController.feather -= 1;
        _shield.SetActive(true);
    }

    public void SetOxygenTank()
    {
        if (_playerController.feather < 1) return;
        _isTankClicked = true;
        if (_isTankClicked)
        {
            _oxygenTankButton.SetActive(false);
        }
        _playerController.feather -= 1;
        _oxygenTank.SetActive(true);
    }

    public void FeatherUp()
    {
        _playerController.feather += 1;
    }

    #endregion

}
