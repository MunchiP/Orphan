using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void EndAttack()
    {
        anim.SetBool("isAttacking", false);
    }
}
