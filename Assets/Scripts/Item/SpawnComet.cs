using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpawnComet : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] GameObject comet;
    [SerializeField] GameObject _backgroundObject; // 배경 오브젝트
    [SerializeField] float _cometSpacing = 25f; // 아이템 간의 기본 간격
    [SerializeField] float _offsetRange = 12.5f; // 간격의 랜덤 오프셋 범위
    [SerializeField] float spawnRate = 3f;

    HeightManager _heightManager;
    Vector2 _backgroundSize;
    float _cometDestroyHeight;
    float _startX, _endX, _endY;
    #endregion

    #region PrivateMethods
    void Awake()
    {
        _heightManager = FindObjectOfType<HeightManager>();
    }


    void Start()
    {
        // 배경 오브젝트의 크기 계산
        BoxCollider2D backgroundCollider = _backgroundObject.GetComponent<BoxCollider2D>();
        _backgroundSize = backgroundCollider.bounds.size;
        _cometDestroyHeight = _heightManager._spaceHeight;

        // 아이템 생성
        StartCoroutine(Spawn());
    }

    void Update()
    {
        _startX = _backgroundObject.transform.position.x - _backgroundSize.x * 0.3f;
        _endX = _backgroundObject.transform.position.x + _backgroundSize.x * 0.3f;
        _endY = _heightManager._cometSpawnY;
    }

    IEnumerator Spawn()
    {
        while (PlayerState._state != PlayerState.State.water)
        {
            for (float x = _startX; x <= _endX; x += _cometSpacing)
            {
                // 위치에 랜덤 오프셋 추가
                float randomOffsetX = Random.Range(-_offsetRange, _offsetRange);
                float randomOffsetY = Random.Range(-_offsetRange, _offsetRange);
                Vector3 spawnPosition = new Vector3(x + randomOffsetX, _endY + randomOffsetY, 0);
                GameObject spawnedItem;
                float randomYSpeed = Random.Range(-5, -9);

                spawnedItem = Instantiate(comet, spawnPosition, Quaternion.identity, _backgroundObject.transform);
                spawnedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, randomYSpeed), ForceMode2D.Impulse);
                spawnedItem.transform.localScale /= new Vector2(500f, 600f);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
    #endregion



}
