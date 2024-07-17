using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    #region PrivateVariables
    Rigidbody2D myRigidbody;
    PlayerController playerController;
    [SerializeField] ParticleSystem waterParticle;
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
            myRigidbody.velocity = Vector2.zero;
            playerController.IsAlive = false;
            waterParticle.Play();
        }
    }
    #endregion
}
