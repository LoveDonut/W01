using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
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
        firstJumpPower = (player.GetComponent<PlayerController>()._jumpDirection.x + player.GetComponent<PlayerController>()._jumpDirection.y) / 2;
        jumpPowerUp = GetComponent<Strengthen>()._jumpPowerUp;
    }

    void Start()
    {
        sunHeight = heightManager.GetComponent<HeightManager>()._sunHeight;
        playerHeight = player.transform.position.y;
    }


    void Update()
    {
        addJumpPower = ((jumpPowerUp.x + jumpPowerUp.y) / 2) + currentJumpPower;
        addMaxHP = player.GetComponent<PlayerController>().maxHP + GetComponent<Strengthen>()._maxHpUp;
        addingJumpPower = ((player.GetComponent<PlayerController>()._jumpDirection.x + player.GetComponent<PlayerController>()._jumpDirection.y) / 2) - firstJumpPower;
        playerHeight = player.transform.position.y;
        currentJumpPower = firstJumpPower + addingJumpPower;

        // 슬라이더
        heightSlider.value = playerHeight / sunHeight;
        healthSlider.maxValue = player.GetComponent<PlayerController>().maxHP;
        healthSlider.value = player.GetComponent<PlayerController>().hp;

        // text 내용 수정
        healthText.text = (int)player.GetComponent<PlayerController>().hp + " / " + player.GetComponent<PlayerController>().maxHP;
        jumpPowerText.text = "JumpPower : " + firstJumpPower + " ( + " + addingJumpPower + " )";
        featherText.text = "" + player.GetComponent<PlayerController>().feather;



        jumpPowerUpText.text = "" + addJumpPower;
        addMaxHPText.text = "" + addMaxHP;
        featherInfoText.text = "" + player.GetComponent<PlayerController>().feather;

        
        currentJumpPowerText.text = "" + currentJumpPower;
        currentMaxHPText.text = "" + player.GetComponent<PlayerController>().maxHP;




        // UI 확인용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            titleUI.SetActive(false);
        }
        if (player.GetComponent<PlayerController>().IsGameStart)
        {
            gameUI.SetActive(true);
            spaceLongTutorial.SetActive(true);
            SpaceLongKey.SetBool("IsGameStart", true);
            SpaceLongArrow.SetBool("IsGameStart", true);
        }
        if (!player.GetComponent<PlayerController>().jumpTutorial)
        {
            spaceLongTutorial.SetActive(false);
            spaceShortTutorial.SetActive(true);
        }
        if (!player.GetComponent<PlayerController>().flyTutorial)
        {
            spaceShortTutorial.SetActive(false);
            shiftTutorial.SetActive(true);
            ShiftKey.SetBool("IsGameStart", true);
            ShiftArrow.SetBool("IsGameStart", true);
        }
        if (!player.GetComponent<PlayerController>().holdTutorial)
        {
            shiftTutorial.SetActive(false);
        }
        if (!player.GetComponent<PlayerController>().IsAlive) // 수정 필요
        {
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
        }
        if (player.GetComponent<PlayerController>().useStamina)
        {
            print("홀드시작");
            staminaObject.SetActive(true);
            staminaTime -= Time.deltaTime;
            staminaSlider.GetComponent<Slider>().value = staminaTime;
        }
        else if (!player.GetComponent<PlayerController>().useStamina)
        {
            print("홀드End");
            staminaObject.SetActive(false);
            staminaTime = 3f;
            staminaSlider.GetComponent<Slider>().value = staminaTime;
        }
    }
}
