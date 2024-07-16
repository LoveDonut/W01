using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    #region PrivateVariables
    Rigidbody2D myRigidbody;
    PlayerController playerController;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();    
        playerController = GetComponent<PlayerController>();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            playerController.IsAlive = false;
        }
    }
    #endregion
}
