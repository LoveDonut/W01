using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{

    public Animator _imageAnim;
    public GameObject gameClear;

    void Awake()
    {
        
    }

    public void EnterSun()
    {
        _imageAnim.SetTrigger("Clear");
    }

    public void ActiveGameClear()
    {
        gameClear.SetActive(true);
    }

}
