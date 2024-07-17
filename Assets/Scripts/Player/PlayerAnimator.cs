using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    public Animator bodyAnim;
    public Animator wingAnim;

    public void BodyBack()
    {
        bodyAnim.SetTrigger("_readyToJump");
    }

    public void BodyRun()
    {
        bodyAnim.SetTrigger("_running");
    }

    public void BodyFly()
    {
        bodyAnim.SetTrigger("_flying");
    }

    public void WingJump()
    {
        wingAnim.SetTrigger("_jump");
    }

    public void WingJumpReset()
    {
        wingAnim.ResetTrigger("_fly");
        wingAnim.ResetTrigger("_glide");
        wingAnim.SetTrigger("_jump");
    }

    public void WingFly()
    {

        wingAnim.SetTrigger("_fly");
    }

    public void WingGlide()
    {
        wingAnim.ResetTrigger("_jump");
        wingAnim.ResetTrigger("_fly");
        wingAnim.SetTrigger("_glide");
    }

}
