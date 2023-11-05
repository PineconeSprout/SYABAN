using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Image HP; 
    public float HPWidth;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private LayerMask blockingLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float health;
    [SerializeField] public Transform firepoint;
    [SerializeField] public bool isShooting;
    public bool HasStamina = true;
    public GameObject bullet;
    private float horizontal;
    private bool isFacingRight = true;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        HPWidth = HP.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Flip();
        if (Input.GetButtonDown("Jump")&& IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (Input.GetButtonDown("Fire1") && HasStamina)
        {
            AttackPewPew();
        }
        if (Input.GetButtonUp("Fire1") && HasStamina)
        {
            animator.SetBool("IsShooting", false);
            isShooting = false;
        }



    }

    void FixedUpdate()
    {
         rb.velocity = new Vector2(horizontal * speed, rb.velocity.y); //movement left and right
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, blockingLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            firepoint.transform.Rotate(0, -180, 0);
        }
    }

    public void TakeDamage(int amount)
    {
        if(health>0)
        {
            health -= amount;
            HPWidth -= amount;
            Vector2 temp = new Vector2(HPWidth, HP.rectTransform.rect.height);
            HP.rectTransform.sizeDelta = temp;
        }
    }

    public void AttackPewPew()
    {
        float timer=5f;
        while (timer > 0)
        {
            animator.SetBool("IsShooting", true); 
            isShooting = true;
            Debug.Log("Attack point reached");
            timer-=Time.deltaTime;
        } 
        
        Instantiate(bullet, firepoint.position,firepoint.rotation);
     
    }

}
