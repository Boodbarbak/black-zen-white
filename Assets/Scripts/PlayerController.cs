using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject startPoint;
    public GameEngine gameEngine;

    public float speed;
    private float move;

    public float jumpForce;
    private bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;

    private float fallTime;
    public float fallAnimationStartFrom = 0.8f;
    private bool isFalling;

    private bool feetOnGround;
    private bool isOnGround;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    [SerializeField] private AudioSource bumpSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource fallSoundEffect;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator an;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * move, rb.velocity.y);
        feetOnGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
    }

    // Update is called once per frame
    void Update()
    {
        if (move > 0) {
            // sr.flipX = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (move < 0)
        {
            // sr.flipX = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (Input.GetButtonDown("Jump") && feetOnGround == true)
        {
            jumpSoundEffect.Play();
            an.Play("Jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }
        else if (Input.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else if (Input.GetButtonUp("Jump"))
        {
            an.Play("Fall");
            isJumping = false;
        }

        if (isOnGround == false && isJumping == false)
        {
            if (isFalling == false)
            {
                isFalling = true;
                fallTime = 0;
            }
            else
            {
                fallTime += Time.deltaTime;
                if (fallTime > fallAnimationStartFrom && !an.GetCurrentAnimatorStateInfo(0).IsName("Scared"))
                {
                    fallSoundEffect.Play();
                    an.Play("Scared");
                }
            }
        }
        
        if (transform.position.y < -20) {
            // gameEngine.Restart();
            BackToStart();
        }
    }

    public void BackToStart()
    {
        transform.position = startPoint.transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            bumpSoundEffect.Play();
            isOnGround = true;
            isFalling = false;
            fallSoundEffect.Stop();
            an.Play("Idle");
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        feetOnGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && feetOnGround == false)
        {
            isOnGround = false;
            if (isJumping == false)
            {
                isFalling = true;
                fallTime = 0;
                // an.Play("Scared");
            }
        }
    }
}
