using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Daehee
public class PlayerState : MonoBehaviour
{
    public enum State
    {
        NotStart,
        LookupSun,
        follow,
        back,
        dash,
        recover,
        toSpace,
        shake,
        water,
        clear
    };

    #region PrivateVariables

    Rigidbody2D _myRigidbody;
    PlayerController _playerController;

    #endregion

    #region PublicVariables

    public static State _state = State.NotStart;

    #endregion

    #region PrivateMethods

    void Awake()
    {
        _myRigidbody = GetComponent<Rigidbody2D>();
        _playerController = GetComponent<PlayerController>();
    }

    #endregion

    #region PublicMethods
    public void SetState(State state)
    {
        _state = state;
    }

    public void GameOver()
    {
        _playerController.hp = 0;
        SetState(State.water);
    }

    #endregion

}
