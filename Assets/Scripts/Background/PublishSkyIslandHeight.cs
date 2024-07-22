using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublishSkyIslandHeight : MonoBehaviour
{
    HeightManager _heightManager;

    void Awake()
    {
        _heightManager = FindObjectOfType<HeightManager>();    
    }

    void Start()
    {
        _heightManager.SetSkyIslandHeight(transform.position.y);
    }
}
