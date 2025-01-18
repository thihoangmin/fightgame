using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] 

    float PlayerHealth = 100f;
    int attackRange = 5;
    public LayerMask EnemyMask;
    float dirX;
    Vector2 Pos;
    bool CanAttack = true;
    bool isDead= false;
    float moveSpeed = 5f;
    public Slider slider;
    Animator animator;
    Rigidbody2D rb;
    Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
       slider.maxValue = PlayerHealth;
       Pos= transform.position;
       rb= GetComponent<Rigidbody2D>(); 
       localScale = transform.localScale;
       animator = GetComponent<Animator>();
        // vector3 StartPos = transform.pos;
    }


    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttack",false);
        yield return new WaitForSeconds(0.5f);
        CanAttack= true;

    }
    // public void Die()
    // {
    //    Debug.log("you die") ;
    // }
    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        PlayerHealth -= damage;
        if (PlayerHealth <= 0)
        {
            animator.SetTrigger("Die");


        }


    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }
    public void Attack()
    {
        CanAttack = false;
        animator.SetBool("IsAttack",true);
        StartCoroutine(DelayAttack());
        Vector3 pos = transform.position;
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, EnemyMask);
        if (colInfo != null)
        {
         colInfo.GetComponent<BossScript>().TakeDamage(20f);
        }
    }

    void Update()
    {
        SetHealth(PlayerHealth);
        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.1)
            rb.AddForce(Vector2.up * 100f);
        if (Input.GetKey(KeyCode.Mouse0) && (CanAttack == true))
            Attack();
        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 6f;
        else
            moveSpeed = 3f;


        if (isDead == false)
        {
            dirX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        }
        SetAnimationState();
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX,rb.velocity.y);
        if ((dirX>0 && localScale.x<0) || (dirX<0 && localScale.x>0))
            localScale.x *= -1;
        transform.localScale = localScale;

    }
    void SetAnimationState()
    {
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
    }
    

}
