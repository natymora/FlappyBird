using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        if (GameController.instance.canPlay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else
            {
                ReturnToIdle();
            }
        }
    }

    public void Jump()
    {
        animator.SetBool("isJump", true);
    }

    public void ReturnToIdle()
    {
        animator.SetBool("isJump", false);
    }
}