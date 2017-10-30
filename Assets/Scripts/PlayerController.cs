using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Use this for initialization
    public float maxSpeed = 10f;
    bool facingRight = true;
    private Rigidbody2D myRigidbody2D;
    Animator anim;
    bool grounded = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float jumpForce = 700f;
    bool doubleJump = false;
    bool flying = false;
    bool isDying = false;
    public Vector3 respawnPosition;
    public int score;
    [SerializeField]
    AudioClip Death;
    [SerializeField]
    AudioClip Ghost_Waltz;
    void Start ()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawnPosition = this.transform.position;
        anim.SetBool("willDie", false);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);
        if (grounded)
        {
            doubleJump = false;
            flying = false;
        }
        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);
        if (isDying == false)
        {
            float move = Input.GetAxis("Horizontal");
            anim.SetFloat("Speed", Mathf.Abs(move));
            myRigidbody2D.velocity = new Vector2(move * maxSpeed, myRigidbody2D.velocity.y);
            if (move > 0 && !facingRight)
                Flip();
            else if (move < 0 && facingRight)
                Flip();
        }
    }
    void Update()
    {
        if (isDying == false)
        {
            if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("Ground", false);
                myRigidbody2D.AddForce(new Vector2(0, jumpForce));
                if (!doubleJump && !grounded)
                    doubleJump = true;
            }
            if ((!grounded && !flying) && Input.GetKeyDown(KeyCode.F))
            {
                myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);
                myRigidbody2D.gravityScale = 0;
                flying = true;
            }
            else if ((!grounded && flying) && Input.GetKeyDown(KeyCode.F))
            {
                myRigidbody2D.velocity = new Vector2(0, 0);
                myRigidbody2D.gravityScale = 1;
                flying = false;
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    public void Die()
    {
        if (isDying == false)
        {
            FindObjectOfType<AudioScript>().PlayAudioClip(Death);
            anim.SetBool("willDie", true);
            isDying = true;
            StartCoroutine(Respawn());
        }
    }
    public IEnumerator Respawn()
    {
        float deathTime = anim.GetCurrentAnimatorClipInfo(0).Length;
        Debug.Log(deathTime);
        yield return new WaitForSeconds(deathTime);
        isDying = false;
        anim.SetBool("willDie", false);
        this.transform.position = respawnPosition;
    }
}
