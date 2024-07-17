using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        shake
    };

    #region PrivateVariables

    Rigidbody2D myRigidbody;

    #endregion

    #region PublicVariables

    public State _state = State.NotStart;

    #endregion

    #region PrivateMethods

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    #region PublicMethods
    public void SetState(State state)
    {
        _state = state;
    }
    #endregion

}
