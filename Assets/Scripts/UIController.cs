using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    PlayerState _playerState;
    float playerHeight;
    float sunHeight;
    float firstJumpPower;
    float addingJumpPower;
    
    public GameObject player;
    public GameObject heightManager;

    [Header("Slider")]
    public Slider heightSlider;
    public Slider healthSlider;

    [Header("Text")]
    public TMP_Text featherText;
    public TMP_Text healthText;
    public TMP_Text jumpPowerText;
    public TMP_Text jumpPowerUpText;
    public TMP_Text AddMaxHPText;

    [Header("UI")]
    public GameObject titleUI;
    public GameObject gameUI;
    public GameObject gameClearUI;
    public GameObject gameOverUI;
    public GameObject subtitleUI;
    public GameObject statusUI;

    void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        firstJumpPower = (player.GetComponent<PlayerController>()._jumpDirection.x + player.GetComponent<PlayerController>()._jumpDirection.y) / 2;
    }

    void Start()
    {
        sunHeight = heightManager.GetComponent<HeightManager>()._sunHeight;
        playerHeight = player.transform.position.y;
    }


    void Update()
    {
        addingJumpPower = ((player.GetComponent<PlayerController>()._jumpDirection.x + player.GetComponent<PlayerController>()._jumpDirection.y) / 2) - firstJumpPower;
        playerHeight = player.transform.position.y;
        heightSlider.value = playerHeight / sunHeight;
        healthSlider.maxValue = player.GetComponent<PlayerController>().maxHP;
        healthSlider.value = player.GetComponent<PlayerController>().hp;

        // text 내용 수정
        healthText.text = (int)player.GetComponent<PlayerController>().hp + " / " + player.GetComponent<PlayerController>().maxHP;
        jumpPowerText.text = "JumpPower : " + firstJumpPower + " ( + " + addingJumpPower + " )";
        featherText.text = "" + player.GetComponent<PlayerController>().feather;

        // UI 확인용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            titleUI.SetActive(false);
        }
        if(player.GetComponent<PlayerController>().IsGameStart)
        {
            gameUI.SetActive(true);
        }
        if (!player.GetComponent<PlayerController>().IsAlive) // 수정 필요
        {
            gameOverUI.SetActive(true);
            gameUI.SetActive(false);
        }
    }
}
