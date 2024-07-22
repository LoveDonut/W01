using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnComet : MonoBehaviour
{
    #region PrivateVariables
    [SerializeField] GameObject comet;
    [SerializeField] GameObject _backgroundObject; // ��� ������Ʈ
    [SerializeField] float _cometSpacing = 25f; // ������ ���� �⺻ ����
    [SerializeField] float _offsetRange = 12.5f; // ������ ���� ������ ����
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
        // ��� ������Ʈ�� ũ�� ���
        BoxCollider2D backgroundCollider = _backgroundObject.GetComponent<BoxCollider2D>();
        _backgroundSize = backgroundCollider.bounds.size;
        _cometDestroyHeight = _heightManager._spaceHeight;

        float startX, endX, endY;

        (startX, endX, endY) = CalculateBackgroundSize();

        // ������ ����
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
                // ��ġ�� ���� ������ �߰�
                float randomOffsetX = Random.Range(-_offsetRange, _offsetRange);
                float randomOffsetY = Random.Range(-_offsetRange, _offsetRange);
                Vector3 spawnPosition = new Vector3(x + randomOffsetX, endY + randomOffsetY, 0);

                // ������ ���� �� �θ� ����
                GameObject spawnedItem = Instantiate(comet, spawnPosition, Quaternion.identity, _backgroundObject.transform);
                spawnedItem.transform.localScale /= new Vector2(500f, 600f);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
    #endregion



}
