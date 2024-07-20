using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthenData : MonoBehaviour
{
    PlayerController playerController;

    public static StrengthenData instance;

    public bool isRestart;
    public float defaultHp = 100f;
    public Vector2 defaultJumpPower;
    public float maxHpUp;
    public int feather;
    public Vector2 jumpPowerUp;
    public float sliderValue;
    public Vector3 _position;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        defaultJumpPower = playerController._jumpDirection;
    }

}
