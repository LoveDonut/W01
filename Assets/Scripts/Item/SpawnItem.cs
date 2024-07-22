using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] float _spawnRateInSpace = 0.75f;
    [SerializeField] GameObject[] _items; // 아이템 프리팹 배열
    [SerializeField] GameObject _backgroundObject; // 배경 오브젝트
    [SerializeField] float _itemSpacing = 50f; // 아이템 간의 기본 간격
    [SerializeField] float _offsetRange = 1f; // 간격의 랜덤 오프셋 범위
    
    HeightManager _heightManager;
    Vector2 _backgroundSize;
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

        // 아이템 생성
        SpawnItems();
    }

    void SpawnItems()
    {
        float startX = _backgroundObject.transform.position.x - _backgroundSize.x * 0.3f;
        float endX = _backgroundObject.transform.position.x + _backgroundSize.x * 0.3f;
        float startY = _backgroundObject.transform.position.y - _backgroundSize.y / 2 + 120f;
        float endY = _backgroundObject.transform.position.y + _backgroundSize.y / 2 + 100f;

        Debug.Log($"start : ({startX} , {startY}) / end : ({endX} , {endY})");
        Debug.Log($"({_backgroundObject.transform.position.y} , {_backgroundSize.y})");

        // spawn under space
        SpawnByHeight(_itemSpacing, startX, endX, startY, endY / 2);

        // spawn on space
        SpawnByHeight(_itemSpacing / _spawnRateInSpace, startX, endX, endY / 2, endY);
    }

    private void SpawnByHeight(float itemSpacing, float startX, float endX, float startY, float endY)
    {
        for (float x = startX; x <= endX; x += itemSpacing)
        {
            for (float y = startY; y <= endY; y += itemSpacing)
            {
                // 랜덤한 아이템 선택
                int randomIndex = Random.Range(0, _items.Length);
                GameObject itemToSpawn = _items[randomIndex];

                // 위치에 랜덤 오프셋 추가
                float randomOffsetX = Random.Range(-_offsetRange, _offsetRange);
                float randomOffsetY = Random.Range(-_offsetRange, _offsetRange);
                Vector3 spawnPosition = new Vector3(x + randomOffsetX, y + randomOffsetY, 0);

                // 아이템 생성 및 부모 설정
                GameObject spawnedItem = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity, _backgroundObject.transform);
                spawnedItem.transform.localScale /= new Vector2(500f, 600f);
            }
        }
    }
    #endregion



}
