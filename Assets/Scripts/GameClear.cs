using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameClear : MonoBehaviour
{
    PlayerState _playerState;
    public Animator _imageAnim;
    public GameObject gameClear;
    public bool _gameQuit;

    void Awake()
    {
        _playerState = FindObjectOfType<PlayerState>();      
        _gameQuit = false;
    }

    private void Update()
    {
        if(_gameQuit && Input.anyKey)
        {

#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();

#endif
        }
    }

    public void EnterSun()
    {
        _imageAnim.SetTrigger("Clear");
        _playerState.SetState(PlayerState.State.clear);
    }

    public void ActiveGameClear()
    {
        gameClear.SetActive(true);
        _gameQuit = true;

    }

}
