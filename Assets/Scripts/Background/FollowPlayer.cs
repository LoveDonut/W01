using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    BoxCollider2D myCollider;

    #region PrivateVariables
    Transform _playerTransform;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
        TryGetComponent<BoxCollider2D>(out myCollider);
    }
    void Update()
    {
        if (myCollider != null && myCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) return;

        Follow();
    }

    void Follow()
    {
        if (PlayerState._state == PlayerState.State.water) return;

        transform.position = new Vector2(_playerTransform.position.x, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    #endregion
}
