using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    FollowCamera followCamera;

    void Awake()
    {
        followCamera = FindObjectOfType<FollowCamera>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            followCamera.HitCameraEffect();
            Destroy(gameObject);
        }
    }
}
