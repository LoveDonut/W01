using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBigComets : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] GameObject comet;
    [SerializeField] GameObject _backgroundObject; // ��� ������Ʈ
    [SerializeField] float _cometSpacingX = 25f; // ������ ���� �⺻ ����
    [SerializeField] float _cometSpacingY = 25f; // ������ ���� �⺻ ����
    [SerializeField] float _offsetRange = 12.5f; // ������ ���� ������ ����
    [SerializeField] GameObject _player;
    FollowCamera _followCamera;
    BoxCollider2D _myCollider;
    HeightManager _heightManager;
    Vector2 _backgroundSize;
    float _cometDestroyHeight;
    float _startX, _endX;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _followCamera = FindObjectOfType<FollowCamera>();
        _myCollider = GetComponent<BoxCollider2D>();
        _heightManager = FindObjectOfType<HeightManager>();
    }


    void Start()
    {
        // ��� ������Ʈ�� ũ�� ���
        BoxCollider2D backgroundCollider = _backgroundObject.GetComponent<BoxCollider2D>();
        _backgroundSize = backgroundCollider.bounds.size;
        _cometDestroyHeight = _heightManager._spaceHeight;
    }

    void Update()
    {
        _startX = _backgroundObject.transform.position.x - _backgroundSize.x * 0.3f;
        _endX = _backgroundObject.transform.position.x + _backgroundSize.x * 0.3f;
    }


    void Spawn()
    {
        for (float x = _startX; x <= _endX; x += _cometSpacingX)
        {
            for(float y = _heightManager._cometSpawnY; y < _heightManager._cometSpawnY + _cometSpacingY; y += _cometSpacingY)
            {
                // ��ġ�� ���� ������ �߰�
                float randomOffsetX = Random.Range(-_offsetRange, _offsetRange);
                float randomOffsetY = Random.Range(-_offsetRange, _offsetRange);
                Vector3 spawnPosition = new Vector3(x + randomOffsetX, y + randomOffsetY, 0);
                GameObject spawnedItem;
                float randomYSpeed = Random.Range(-5, -9);

                spawnedItem = Instantiate(comet, spawnPosition, Quaternion.identity, _backgroundObject.transform);
                spawnedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, randomYSpeed), ForceMode2D.Impulse);
                spawnedItem.transform.localScale /= new Vector2(500f, 600f);    
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Spawn();
            PlayerState._state = PlayerState.State.cometEvent;
            gameObject.SetActive(false);
        }
    }
    #endregion

}
