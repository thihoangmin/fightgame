using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float dirX;
    bool isDead= false;
    float moveSpeed = 5f;
    Animator animator;
    Rigidbody2D rb;
    Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
       rb= GetComponent<Rigidbody2D>(); 
       localScale = transform.localScale;
       animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 6f;
        else
            moveSpeed = 3f;

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.1)
            rb.AddForce(Vector2.up * 1000f);
        if (isDead == false)
        {
            dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }
        SetAnimationState();
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX,0);
        if ((dirX>0 && localScale.x<0) || (dirX<0 && localScale.x>0))
            localScale.x *= -1;
        transform.localScale = localScale;

    }
    void SetAnimationState()
    {
        Debug.Log(dirX);
        if (dirX == 0)
            animator.SetBool("IsWalk",false);
            animator.SetBool("IsRun",false);
        if (Mathf.Abs(rb.velocity.y) < 0.1)
            animator.SetBool("IsJump",false);
        else
            animator.SetBool("IsJump",true);
        if ((Mathf.Abs(dirX) <= 5 && Mathf.Abs(dirX) > 0) && (Mathf.Abs(rb.velocity.y) < 0.1))
        {
            animator.SetBool("IsWalk",true);
        }

        if ((Mathf.Abs(dirX) > 5) && (Mathf.Abs(rb.velocity.y) < 0.1))
        {
            animator.SetBool("IsRun",true);
        }
         if (Input.GetMouseButtonDown(0))
         {
            animator.SetBool("IsAttack",true);
            WaitForSeconds(1);
         }
         else 
         (
            if (!Input.GetMouseButtonDown(0))
            animator.SetBool("IsAttack",false);
         )
            
         



    
            
        

    }
    

}
