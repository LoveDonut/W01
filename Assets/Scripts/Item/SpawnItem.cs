using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] float _spawnRateInSpace = 0.75f;
    [SerializeField] GameObject[] _items; // ������ ������ �迭
    [SerializeField] GameObject _backgroundObject; // ��� ������Ʈ
    [SerializeField] float _itemSpacing = 50f; // ������ ���� �⺻ ����
    [SerializeField] float _offsetRange = 1f; // ������ ���� ������ ����
    
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
        // ��� ������Ʈ�� ũ�� ���
        BoxCollider2D backgroundCollider = _backgroundObject.GetComponent<BoxCollider2D>();
        _backgroundSize = backgroundCollider.bounds.size;

        // ������ ����
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
                // ������ ������ ����
                int randomIndex = Random.Range(0, _items.Length);
                GameObject itemToSpawn = _items[randomIndex];

                // ��ġ�� ���� ������ �߰�
                float randomOffsetX = Random.Range(-_offsetRange, _offsetRange);
                float randomOffsetY = Random.Range(-_offsetRange, _offsetRange);
                Vector3 spawnPosition = new Vector3(x + randomOffsetX, y + randomOffsetY, 0);

                // ������ ���� �� �θ� ����
                GameObject spawnedItem = Instantiate(itemToSpawn, spawnPosition, Quaternion.identity, _backgroundObject.transform);
                spawnedItem.transform.localScale /= new Vector2(500f, 600f);
            }
        }
    }
    #endregion



}
