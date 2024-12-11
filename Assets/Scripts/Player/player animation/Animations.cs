using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator playerAnimator;
    public AttackAnimation Attack;

    public void Idle(Animator animator)
    {
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("Idle", true);

    }

    public void WalkingAnimation(Animator animator)
    {

        animator.SetBool("Idle", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isWalking", true);

    }

    public void JumpingAnimation(Animator animator)
    {

        animator.SetBool("Idle", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isJumping", true);

    }

    public void FallingAnimation(Animator animator)
    {

        animator.SetBool("Idle", false);
        animator.SetBool("isFalling", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", true);

    }
    public void AttackAnimation(Animator animator)
    {
        animator.SetInteger("tapCount", Attack.tapCount);
    }

}

