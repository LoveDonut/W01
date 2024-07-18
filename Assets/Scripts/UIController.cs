using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    PlayerController playerController;
    PlayerState _playerState;
    Vector2 jumpPowerUp;
    float playerHeight;
    float sunHeight;
    float firstJumpPower;
    float addingJumpPower;
    float addMaxHP;
    float addJumpPower;
    float currentJumpPower;
    float staminaTime = 3;

    public GameObject player;
    public GameObject heightManager;
    public GameObject spaceLongTutorial;
    public GameObject spaceShortTutorial;
    public GameObject shiftTutorial;
    public GameObject staminaObject;
    public Strengthen strengthen;

    [Header("Animator")]
    public Animator SpaceLongKey;
    public Animator SpaceLongArrow;
    public Animator SpaceShortKey;
    public Animator SpaceShortArrow;
    public Animator ShiftKey;
    public Animator ShiftArrow;

    [Header("Button")]
    public GameObject jumpPowerUpBtn;
    public GameObject addMaxHPBtn;

    [Header("Slider")]
    public Slider bestHeightSlider;
    public Slider heightSlider;
    public Slider healthSlider;
    public Slider staminaSlider;

    [Header("Text")]
    public TMP_Text featherText;
    public TMP_Text healthText;
    public TMP_Text jumpPowerText;
    public TMP_Text currentJumpPowerText;
    public TMP_Text currentMaxHPText;
    public TMP_Text jumpPowerUpText;
    public TMP_Text addMaxHPText;
    public TMP_Text featherInfoText;

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
        jumpPowerUp = GetComponent<Strengthen>()._jumpPowerUp;
        strengthen = FindObjectOfType<Strengthen>();
        playerController = FindObjectOfType<PlayerController>();

        if (!StrengthenData.instance.isRestart)
        {
            titleUI.SetActive(true);
        }
        else
        {
            statusUI.SetActive(true);
            bestHeightSlider.GetComponent<Slider>().value = StrengthenData.instance.sliderValue;
        }
    }

    void Start()
    {
        sunHeight = heightManager.GetComponent<HeightManager>()._sunHeight;
        playerHeight = player.transform.position.y;
        currentJumpPower = (playerController._jumpDirection.x + playerController._jumpDirection.y) / 2f;
        addJumpPower = currentJumpPower;
    }


    void Update()
    {
        currentJumpPower = (playerController._jumpDirection.x + playerController._jumpDirection.y) / 2f;
        addJumpPower = ((jumpPowerUp.x + jumpPowerUp.y) / 2) + currentJumpPower;
        addMaxHP = player.GetComponent<PlayerController>().maxHP + GetComponent<Strengthen>()._maxHpUp;
        playerHeight = player.transform.position.y;

        // �����̴�
        heightSlider.value = playerHeight / sunHeight;
        healthSlider.maxValue = player.GetComponent<PlayerController>().maxHP;
        healthSlider.value = player.GetComponent<PlayerController>().hp;

        // text ���� ����
        healthText.text = (int)player.GetComponent<PlayerController>().hp + " / " + player.GetComponent<PlayerController>().maxHP;
        if(StrengthenData.instance != null)
        {
            float powerUpOffset = ((playerController._jumpDirection - StrengthenData.instance.defaultJumpPower).y + (playerController._jumpDirection - StrengthenData.instance.defaultJumpPower).x) / 2f;
            jumpPowerText.text = "JumpPower : " + (StrengthenData.instance.defaultJumpPower.x + StrengthenData.instance.defaultJumpPower.y) / 2 + " ( + " + powerUpOffset + " )";
        }
        else
        {
            jumpPowerText.text = "JumpPower : " + currentJumpPower + " ( + 0 )";
        }
        featherText.text = "" + player.GetComponent<PlayerController>().feather;



        jumpPowerUpText.text = "" + addJumpPower;
        addMaxHPText.text = "" + addMaxHP;
        featherInfoText.text = "" + player.GetComponent<PlayerController>().feather;

        
        currentJumpPowerText.text = "" + currentJumpPower;
        currentMaxHPText.text = "" + player.GetComponent<PlayerController>().maxHP;




        // UI Ȯ�ο�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            titleUI.SetActive(false);
        }
        if (player.GetComponent<PlayerController>().IsGameStart)
        {
            gameUI.SetActive(true);
            if (!StrengthenData.instance.isRestart)
            {
                spaceLongTutorial.SetActive(true);
                SpaceLongKey.SetBool("IsGameStart", true);
                SpaceLongArrow.SetBool("IsGameStart", true);
            }
        }
        if (!player.GetComponent<PlayerController>().jumpTutorial && !StrengthenData.instance.isRestart)
        {
            spaceLongTutorial.SetActive(false);
            spaceShortTutorial.SetActive(true);
        }
        if (!player.GetComponent<PlayerController>().flyTutorial && !StrengthenData.instance.isRestart)
        {
            spaceShortTutorial.SetActive(false);
            shiftTutorial.SetActive(true);
            ShiftKey.SetBool("IsGameStart", true);
            ShiftArrow.SetBool("IsGameStart", true);
        }
        if (!player.GetComponent<PlayerController>().holdTutorial && !StrengthenData.instance.isRestart)
        {
            shiftTutorial.SetActive(false);
        }
        if (!player.GetComponent<PlayerController>().IsAlive)
        {
            spaceLongTutorial.SetActive(false);
            spaceShortTutorial.SetActive(false);
            shiftTutorial.SetActive(false);
            gameOverUI.SetActive(true);
            gameUI.SetActive(false);
        }
        if (player.GetComponent<PlayerController>().feather <= 0)
        {
            jumpPowerUpBtn.GetComponent<Button>().interactable = false;
            addMaxHPBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            jumpPowerUpBtn.GetComponent<Button>().interactable = true;
            addMaxHPBtn.GetComponent<Button>().interactable = true;
        }
        if (bestHeightSlider.GetComponent<Slider>().value < heightSlider.GetComponent<Slider>().value)
        {
            bestHeightSlider.GetComponent<Slider>().value = heightSlider.GetComponent<Slider>().value;
            StrengthenData.instance.sliderValue = bestHeightSlider.GetComponent<Slider>().value;
        }
        if (player.GetComponent<PlayerController>().useStamina)
        {
            staminaObject.SetActive(true);
            staminaTime -= Time.deltaTime;
            staminaSlider.GetComponent<Slider>().value = staminaTime;
        }
        else if (!player.GetComponent<PlayerController>().useStamina)
        {
            staminaObject.SetActive(false);
            staminaTime = 3f;
            staminaSlider.GetComponent<Slider>().value = staminaTime;
        }
    }

    public void ChangeStateAfterRestart()
    {
        PlayerState._state = PlayerState.State.NotStart;
        gameUI.SetActive(true);
    }
}
