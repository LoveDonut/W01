using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublishSunHeight : MonoBehaviour
{
    #region PrivateVariables

    FollowCamera _followCamera;
    UIController _uiController;

    #endregion

    #region PrivateMethods

    void Awake()
    {
        _followCamera = FindObjectOfType<FollowCamera>();
        _uiController = FindObjectOfType<UIController>();    
    }

    void Start()
    {
        _followCamera._sunTransform = transform;
        _uiController.sunHeight = transform.position.y;
    }

    #endregion


}
