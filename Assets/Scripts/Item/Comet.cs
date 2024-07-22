using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Comet : MonoBehaviour
{
    [SerializeField] ParticleSystem deadParticleSystem;
    [SerializeField] float _cometPower = 20f;
    [SerializeField] bool _isBig = false;

    GameObject _background;
    Rigidbody2D cometRb;
    PlayerState playerState;
    FollowCamera followCamera;
    HeightManager _heightManager;
    PlayerController _playerController;

    void Awake()
    {
        _background = FindObjectOfType<BackgroundMove>().gameObject;
        playerState = FindObjectOfType<PlayerState>();
        followCamera = FindObjectOfType<FollowCamera>();
        cometRb = GetComponent<Rigidbody2D>();
        _heightManager = FindObjectOfType<HeightManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update(){
        float middleSize = _background.GetComponent<BoxCollider2D>().bounds.size.x * 0.3f;

        if (transform.position.x <= _background.transform.position.x - middleSize || transform.position.x >= _background.transform.position.x + middleSize)
        {
            Destroy(gameObject);
        }
        if(transform.position.y <= _heightManager._spaceHeight){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Shield"))
        {
            ParticleSystem instance = Instantiate(deadParticleSystem, transform.position, Quaternion.identity);
            Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
            cometRb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player")) {
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.down * _cometPower, ForceMode2D.Impulse);
            followCamera.HitCameraEffect();

            if(_isBig)
            {
                ParticleSystem instance = Instantiate(deadParticleSystem, other.transform.position, Quaternion.identity);
                Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
                _playerController.Damage(10000f);
            }
            else
            {
                ParticleSystem instance = Instantiate(deadParticleSystem, transform.position, Quaternion.identity);
                Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
                cometRb.velocity = Vector2.zero;
                Destroy(gameObject);
            }
        }
        else if(other.CompareTag("Comet") && other.CompareTag("Wind") && _isBig)
        {
            ParticleSystem instance = Instantiate(deadParticleSystem, transform.position, Quaternion.identity);
            Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
            Destroy(other.gameObject);
        }
    }
}