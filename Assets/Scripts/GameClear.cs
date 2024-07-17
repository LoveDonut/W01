using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    PlayerState _playerState;
    public Animator _imageAnim;
    public GameObject gameClear;

    void Awake()
    {
        _playerState = FindObjectOfType<PlayerState>();        
    }

    public void EnterSun()
    {
        _imageAnim.SetTrigger("Clear");
        _playerState.SetState(PlayerState.State.clear);
    }

    public void ActiveGameClear()
    {
        gameClear.SetActive(true);
    }

}
