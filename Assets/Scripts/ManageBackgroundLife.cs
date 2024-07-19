using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBackgroundLife : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] GameObject _background;

    Transform _playerTransform;
    public float _destroyPositionX;
    public float _spawnPositionX;

    #endregion

    #region PrivateMethods
    void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    void Start()
    {
        _spawnPositionX = transform.position.x + transform.localScale.x * 0.25f;
    }

    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        if (_playerTransform.position.x > _spawnPositionX)
        {
            Debug.Log("spawned");
            Instantiate(_background, transform.position + new Vector3(transform.localScale.x * 0.5f, 0f, 0f), Quaternion.identity);
            Destroy(gameObject);
        }
    }
    #endregion
}
