using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    float playerHeight;
    float sunHeight;

    public TMP_Text FeatherText;

    public Slider heightSlider;
    public Slider healthSlider;
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        sunHeight = 300f;
        playerHeight = playerObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        playerHeight = playerObject.transform.position.y;
        heightSlider.value = playerHeight / sunHeight;
        healthSlider.value = playerObject.GetComponent<PlayerController>().hp;
    }
}
