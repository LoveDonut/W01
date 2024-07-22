using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region PrivateVariables

    [SerializeField] int _shieldHp = 2;

    #endregion

    #region PrivateMethods
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Comet"))
        {
            _shieldHp--;
        }

        if(_shieldHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
