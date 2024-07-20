using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    #region PrivateVariables

    Transform _playerTransform;
    Rigidbody2D _playerRigidbody;
    float xLength;

    #endregion

    #region PrivateMethods

    void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        _playerRigidbody = FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        xLength = transform.localScale.x;    
    }

    void Update()
    {
        MoveWithPlayer();    
    }

    void MoveWithPlayer()
    {
        if(_playerRigidbody.velocity.x > 0)
        {
            if (_playerTransform.position.x > transform.position.x + xLength * 0.4f)
            {
                transform.position += new Vector3(xLength * 0.8f, 0f);
            }
        }
        else
        {
            if (_playerTransform.position.x < transform.position.x - xLength * 0.4f)
            {
                transform.position -= new Vector3(xLength * 0.8f, 0f);
            }
        }
    }

    #endregion

}
