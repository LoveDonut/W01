using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour
{
    private Rigidbody2D cometRb;

    void Start()
    {
        cometRb = GetComponent<Rigidbody2D>();
        cometRb.AddForce(Vector2.down*7,ForceMode2D.Impulse);
    }

    void Update(){
        if(transform.position.y <= 750.0f){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}