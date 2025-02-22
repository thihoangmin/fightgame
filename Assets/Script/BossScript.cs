using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class BossScript : MonoBehaviour
{
    public float PlayerHealth;
    public Transform Player;
    public Transform image;
    public Slider PlayerSlider;
    public Slider slider;
    bool isDead = false;
    int count = 0;
    [SerializeField]
    float BossHealth;

    public LayerMask PlayerMask;

    int attackRange = 3;



    Vector2 Pos;
    bool CanAttack = true;
    Animator animator;
    Vector3 localScale;
    float dirX = 1f;
    SpriteRenderer color;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 50;
        slider.value = BossHealth;
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        animator = GetComponent<Animator>();
        color = GetComponent<SpriteRenderer>();
        Pos = transform.position;
        image.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttack",false);
        yield return new WaitForSeconds(1f);
        CanAttack= true;

    }
    public void SetHealth(float health)
    {
        slider.value = health;  
    }
    public void Win()
    {
        color.enabled = false;
        this.enabled = false;
        Player.GetComponent<PlayerScript>().enabled = false;
        slider.enabled = false;
        PlayerSlider.enabled = false;
        image.GetComponent<SpriteRenderer>().enabled = true;


    }
    public void TakeDamage(float damage)
    {
        BossHealth -= damage;
        SetHealth(BossHealth);
        if (BossHealth <= 0)
        {
            if (count <= 2)
            {
                if (!isDead)
                    isDead = true;
                count += 1;
                StartCoroutine(DelayDie());
            }
            else
            {
                Win();
            }
        }

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
            if ((PlayerHealth >= 0))
            {
                colInfo.GetComponent<PlayerScript>().TakeDamage(20f);
            }
        }


    }
    void Update()
    {
        SetAnimationState();


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
    IEnumerator DelayDie()
    {
        animator.SetBool("IsDie", true);
        Debug.Log("Set true");
        yield return new WaitForSeconds(0.2f);
        transform.position = Pos;
        animator.SetBool("IsDie", false);
        Restart();
    }
    public void Restart()
    {

        isDead = false;
        BossHealth = 30 + count * 10;
        SetHealth(BossHealth);




    }
    void SetAnimationState()
    {
        if ((Mathf.Abs(dirX) <= 5 && Mathf.Abs(dirX) > 0) && (Mathf.Abs(rb.velocity.y) < 0.1))
        {
            animator.SetBool("IsWalk", true);

        }
    }
}
