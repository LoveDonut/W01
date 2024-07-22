using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Comet : MonoBehaviour
{
    [SerializeField] ParticleSystem deadParticleSystem;
    [SerializeField] float _cometPower = 30f;

    Rigidbody2D cometRb;
    PlayerState playerState;
    FollowCamera followCamera;
    HeightManager _heightManager;

    void Awake()
    {
        playerState = FindObjectOfType<PlayerState>();
        followCamera = FindObjectOfType<FollowCamera>();
        cometRb = GetComponent<Rigidbody2D>();
        _heightManager = FindObjectOfType<HeightManager>();
    }

    void Start()
    {
        float randomYSpeed = Random.Range(-5, -9);
        cometRb.AddForce(new Vector2(-2,randomYSpeed),ForceMode2D.Impulse);
    }

    void Update(){
        if(transform.position.y <= _heightManager._spaceHeight){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") || other.CompareTag("Comet")) {
            if(other.CompareTag("Player"))
            {
                cometRb.AddForce(Vector2.down * _cometPower, ForceMode2D.Impulse);
                followCamera.HitCameraEffect();
            }
            ParticleSystem instance = Instantiate(deadParticleSystem, transform.position, Quaternion.identity);
            Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
            cometRb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }
}