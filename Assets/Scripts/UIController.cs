using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    PlayerController _playerController;
    PlayerState _playerState;
    Strengthen strengthen;
    FollowCamera _followCamera;
    Vector2 jumpPowerUp;
    float playerHeight;
    float firstJumpPower;
    float addingJumpPower;
    float addMaxHP;
    float addJumpPower;
    float currentJumpPower;
    float staminaTime = 3;
    float _staminaValue;

    public GameObject heightManager;
    public GameObject spaceLongTutorial;
    public GameObject spaceShortTutorial;
    public GameObject shiftTutorial;
    public GameObject staminaObject;
    public float sunHeight;

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
    public TMP_Text featherTextInActiveItem;
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
    public GameObject ActiveItemUI;

    void Awake()
    {
        strengthen = FindObjectOfType<Strengthen>();
        _playerState = GetComponent<PlayerState>();
        jumpPowerUp = strengthen._jumpPowerUp;
        _playerController = FindObjectOfType<PlayerController>();
        _followCamera = FindObjectOfType<FollowCamera>();

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
        playerHeight = _playerController.transform.position.y;
        currentJumpPower = (_playerController._jumpDirection.x + _playerController._jumpDirection.y) / 2f;
        addJumpPower = currentJumpPower;
    }


    void Update()
    {
        currentJumpPower = (_playerController._jumpDirection.x + _playerController._jumpDirection.y) / 2f;
        addJumpPower = ((jumpPowerUp.x + jumpPowerUp.y) / 2) + currentJumpPower;
        addMaxHP = _playerController.maxHP + strengthen._maxHpUp;
        playerHeight = _playerController.transform.position.y;

        heightSlider.value = playerHeight / sunHeight;
        healthSlider.maxValue = _playerController.maxHP;
        healthSlider.value = _playerController.hp;

        // text
        healthText.text = (int)_playerController.hp + " / " + _playerController.maxHP;
        if (StrengthenData.instance != null)
        {
            float powerUpOffset = ((_playerController._jumpDirection - StrengthenData.instance.defaultJumpPower).y + (_playerController._jumpDirection - StrengthenData.instance.defaultJumpPower).x) / 2f;
            jumpPowerText.text = "JumpPower : " + (StrengthenData.instance.defaultJumpPower.x + StrengthenData.instance.defaultJumpPower.y) / 2 + " ( + " + powerUpOffset + " )";
        }
        else
        {
            jumpPowerText.text = "JumpPower : " + currentJumpPower + " ( + 0 )";
        }
        featherText.text = "" + _playerController.feather;
        if (featherTextInActiveItem != null)
        {
            featherTextInActiveItem.text = "" + _playerController.feather;
        }


        jumpPowerUpText.text = "" + addJumpPower;
        addMaxHPText.text = "" + addMaxHP;
        featherInfoText.text = "" + _playerController.feather;

        
        currentJumpPowerText.text = "" + currentJumpPower;
        currentMaxHPText.text = "" + _playerController.maxHP;




        // UI
        if (Input.GetKeyDown(KeyCode.Space))
        {
            titleUI.SetActive(false);
        }
        if (_playerController.IsGameStart)
        {
            gameUI.SetActive(true);
            if (!StrengthenData.instance.isRestart)
            {
                spaceLongTutorial.SetActive(true);
                SpaceLongKey.SetBool("IsGameStart", true);
                SpaceLongArrow.SetBool("IsGameStart", true);
            }
        }
        if (!_playerController.jumpTutorial && !StrengthenData.instance.isRestart)
        {
            spaceLongTutorial.SetActive(false);
            spaceShortTutorial.SetActive(true);
        }
        if (!_playerController.flyTutorial && !StrengthenData.instance.isRestart)
        {
            spaceShortTutorial.SetActive(false);
            shiftTutorial.SetActive(true);
            ShiftKey.SetBool("IsGameStart", true);
            ShiftArrow.SetBool("IsGameStart", true);
        }
        if (!_playerController.holdTutorial && !StrengthenData.instance.isRestart)
        {
            shiftTutorial.SetActive(false);
        }
        if (!_playerController.IsAlive)
        {
            spaceLongTutorial.SetActive(false);
            spaceShortTutorial.SetActive(false);
            shiftTutorial.SetActive(false);
            gameOverUI.SetActive(true);
            gameUI.SetActive(false);
        }
        if (_playerController.feather <= 0)
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
        if (_playerController.useStamina)
        {
            staminaTime -= Time.deltaTime;
            staminaSlider.GetComponent<Slider>().value = staminaTime;
        }
        else if (!_playerController.useStamina && _playerController._holdCoolTime <= 0)
        {
            staminaTime = Mathf.Clamp(staminaTime + Time.deltaTime / 2f, 0f, 3f);
            staminaSlider.GetComponent<Slider>().value = staminaTime;
        }

        _staminaValue = staminaSlider.GetComponent<Slider>().value;

    }

    public float GetStamina()
    {
        return _staminaValue;
    }

    public void ChangeStateAfterRestart()
    {
        PlayerState._state = PlayerState.State.NotStart;
        gameUI.SetActive(true);
    }

    public void AfterSelectItem()
    {
        _playerController._selectItem = false;
        ActiveItemUI.SetActive(false);
        _followCamera.cameraState = FollowCamera.CameraState.moveToSun;
        _playerController.MovePlayerToCenterofSkyIsland();
    }

    public void TurnOnActiveItemUI()
    {
        ActiveItemUI.SetActive(true);
    }
}
