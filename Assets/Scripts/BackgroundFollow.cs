using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (PlayerState._state == PlayerState.State.water) return;
        transform.position = new Vector2(player.transform.position.x + 9, transform.position.y);
    }
}
