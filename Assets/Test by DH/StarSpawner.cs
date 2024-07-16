using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    [SerializeField] GameObject _star;
    [SerializeField] float _moveSpeed = -1f;
    [SerializeField] float _maintainTime = 10f;
    [SerializeField] float _spawnInterval = 0.5f;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            float spawnYInterval = (topRight.y - bottomLeft.y) / 4f;
            float spawnXInterval = (topRight.x - bottomLeft.x) / 4f;

            for (int i = 0; i < 12; i++)
            {
                Vector3 spawnFromRight = new Vector3(topRight.x + 1f, bottomLeft.y + spawnYInterval * i + Random.Range(0, spawnYInterval));
                GameObject instance = Instantiate(_star, spawnFromRight, Quaternion.identity);
                instance.GetComponent<Rigidbody2D>().velocity = new Vector3(_moveSpeed, 0);
                Destroy(instance, _maintainTime);
            }
        }

    }
}
