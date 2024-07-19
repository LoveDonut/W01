using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    #region PrivateVariables
    Transform _playerTransform;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }
    void Update()
    {
        if (PlayerState._state == PlayerState.State.water) return;

        transform.position = new Vector2(_playerTransform.position.x, transform.position.y);
    }
    #endregion
}
