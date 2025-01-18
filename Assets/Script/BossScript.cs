using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public Transform Player;

    [SerializeField] 
    float BossHealth = 10000f;

    public LayerMask PlayerMask;

    int attackRange = 5;
    



    bool CanAttack = true;
    Animator animator;
    Vector3 localScale;
    float dirX = 1f;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttack",false);
        yield return new WaitForSeconds(1f);
        CanAttack= true;

    }

    public void TakeDamage(float damage)
    {
        BossHealth -= damage;
    }
    public void Attack()
    {
        CanAttack = false;
        animator.SetBool("IsAttack",true);
        StartCoroutine(DelayAttack());
        Vector3 pos = transform.position;
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, PlayerMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<PlayerScript>().TakeDamage(20f);
        }


    }
    void Update()
    {
        Debug.Log(CanAttack);
        if ((Mathf.Abs(transform.position.x - Player.position.x) < 1.5f) && CanAttack == true)
        {  
            Attack();
        }
        rb.velocity = new Vector2(dirX,0);
        if ((localScale.x>0) && (transform.position.x < Player.position.x))
        {
            localScale.x *= -1;
            dirX = -dirX;
        }
       
        if ((localScale.x<0) && (transform.position.x > Player.position.x))
        {
             localScale.x *= -1;
             dirX = -dirX;
        }
            
        transform.localScale = localScale;
    }
}
