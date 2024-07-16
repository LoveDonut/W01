using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    FollowCamera followCamera;
    [SerializeField] ParticleSystem deadParticleSystem;

    void Awake()
    {
        followCamera = FindObjectOfType<FollowCamera>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ParticleSystem instance = Instantiate(deadParticleSystem, transform.position, Quaternion.identity);
            Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
            followCamera.HitCameraEffect();
            Destroy(gameObject);
        }
    }
}
