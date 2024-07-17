using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Comet : MonoBehaviour
{
    [SerializeField] ParticleSystem deadParticleSystem;

    private Rigidbody2D cometRb;
    private PlayerState playerState;
    private FollowCamera followCamera;

    void Start()
    {
        playerState = FindObjectOfType<PlayerState>();
        followCamera = FindObjectOfType<FollowCamera>();
        cometRb = GetComponent<Rigidbody2D>();
        float randomYSpeed = Random.Range(-5, -9);
        cometRb.AddForce(new Vector2(-2,randomYSpeed),ForceMode2D.Impulse);
    }

    void Update(){
        if(transform.position.y <= 430.0f){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            ParticleSystem instance = Instantiate(deadParticleSystem, transform.position, Quaternion.identity);
            Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
            followCamera.HitCameraEffect();
            cometRb.velocity = Vector2.zero;
            cometRb.AddForce(Vector2.down*7, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
}