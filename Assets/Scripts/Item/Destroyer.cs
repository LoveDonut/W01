using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(player.transform.position.x-200, transform.position.y);
    }

    void OnTriggerEnter2D(Collider2D other){
        Destroy(other.gameObject);
    }
}
