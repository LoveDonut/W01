using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        float startX, endX, endY;

        (startX, endX, endY) = CalculateBackgroundSize();

        // 아이템 생성
        StartCoroutine(Spawn(startX, endX, endY));
    }

    (float, float, float) CalculateBackgroundSize()
    {
        float startX = _backgroundObject.transform.position.x - _backgroundSize.x / 2;
        float endX = _backgroundObject.transform.position.x + _backgroundSize.x / 2;
        float endY = _backgroundObject.transform.position.y + _backgroundSize.y / 2;
        return (startX, endY, endY);
    }

    IEnumerator Spawn(float startX, float endX, float endY)
    {
        while (PlayerState._state != PlayerState.State.water)
        {
            for (float x = startX; x <= endX; x += _cometSpacing)
            {
                // 위치에 랜덤 오프셋 추가
                float randomOffsetX = Random.Range(-_offsetRange, _offsetRange);
                float randomOffsetY = Random.Range(-_offsetRange, _offsetRange);
                Vector3 spawnPosition = new Vector3(x + randomOffsetX, endY + randomOffsetY, 0);

                // 아이템 생성 및 부모 설정
                GameObject spawnedItem = Instantiate(comet, spawnPosition, Quaternion.identity, _backgroundObject.transform);
                spawnedItem.transform.localScale /= new Vector2(500f, 600f);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
    #endregion



}
