using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrengthenData : MonoBehaviour
{
    public static StrengthenData instance;

    public bool isRestart;
    public float maxHpUp;
    public int feather;
    public Vector2 jumpPowerUp;

    // Start is called before the first frame update
    void Start()
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

}
