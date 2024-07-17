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
    
    public GameObject player;
    public GameObject sun;

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
    public GameObject subtitleUI;
    public GameObject statusUI;

    void Awake()
    {
        _playerState = GetComponent<PlayerState>();
    }

    void Start()
    {
        sunHeight = sun.GetComponent<Transform>().position.y;
        playerHeight = player.transform.position.y;
    }


    void Update()
    {
        playerHeight = player.transform.position.y;
        heightSlider.value = playerHeight / sunHeight;
        healthSlider.maxValue = player.GetComponent<PlayerController>().maxHP;
        healthSlider.value = player.GetComponent<PlayerController>().hp;

        // text ���� ����
        healthText.text = (int)player.GetComponent<PlayerController>().hp + " / " + player.GetComponent<PlayerController>().maxHP;
        jumpPowerText.text = "JumpPower : " + (player.GetComponent<PlayerController>()._jumpDirection.x + player.GetComponent<PlayerController>()._jumpDirection.y) / 2;
        featherText.text = "" + player.GetComponent<PlayerController>().feather;

        // UI Ȯ�ο�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            titleUI.SetActive(false);
        }
        if(player.GetComponent<PlayerController>().IsGameStart)
        {
            gameUI.SetActive(true);
        }
    }
}
