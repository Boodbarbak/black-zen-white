using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject startPoint;
    public GameEngine gameEngine;
    public GameObject bestWishes;

    public float speed;
    private float move;

    public float jumpForce;
    private bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;

    private float fallTime;
    public float fallAnimationStartAfter = 0.6f;
    private bool isFalling;
    public int fallLimit = -20;

    private bool feetOnGround;
    private bool isOnGround;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    [SerializeField] private AudioSource bumpSoundEffect;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource fallSoundEffect;
    private float bumpSoundEffectPitch;
    private float jumpSoundEffectPitch;
    private float fallSoundEffectPitch;

    private Rigidbody2D rb;
    // private SpriteRenderer sr;
    private Animator an;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();
        bumpSoundEffectPitch = bumpSoundEffect.pitch;
        jumpSoundEffectPitch = jumpSoundEffect.pitch;
        fallSoundEffectPitch = fallSoundEffect.pitch;
        bestWishes.SetActive(false);
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
        if (move > 0)
        {
            // sr.flipX = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (move < 0)
        {
            // sr.flipX = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        // Start or continue jumping
        if (Input.GetButtonDown("Jump") || Input.GetButton("Jump"))
        {
            Jump();
        }
        // Stop jumping and start falling
        else if (Input.GetButtonUp("Jump") && isJumping)
        {
            StopJump();
        }

        // Fall if not grounded and not jumping
        if (isOnGround == false && isJumping == false)
        {
            Fall();
        }

        if (move != 0 && an.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            an.Play("Move");
        }
        else if (move == 0 && an.GetCurrentAnimatorStateInfo(0).IsName("Move"))
        {
            an.Play("Idle");
        }

        if (transform.position.y < fallLimit)
        {
            // gameEngine.Restart();
            BackToStart();
        }
    }

    private void Jump()
    {
        // Start jumping from the ground
        if (Input.GetButtonDown("Jump") && isOnGround == true)
        {
            jumpSoundEffect.pitch = jumpSoundEffectPitch + Random.Range(-0.15f, 0.15f);
            jumpSoundEffect.Play();
            an.Play("Jump");
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;

            return;
        }

        if (!isJumping == true || !Input.GetButton("Jump"))
        {
            return;
        }

        // Jump higher or stop jumping if already as high as possible
        if (jumpTimeCounter > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpTimeCounter -= Time.deltaTime;

            return;
        }

        StopJump();
    }

    private void StopJump()
    {
        an.Play("Fall");
        isJumping = false;
    }

    private void Fall()
    {
        // Start falling
        if (isFalling == false)
        {
            isFalling = true;
            fallTime = 0;
            return;
        }

        // Start fall animation and sound when it is time to
        fallTime += Time.deltaTime;
        if (fallTime > fallAnimationStartAfter && !an.GetCurrentAnimatorStateInfo(0).IsName("Scared"))
        {
            fallSoundEffect.pitch = fallSoundEffectPitch + Random.Range(-0.15f, 0.15f);
            fallSoundEffect.Play();
            an.Play("Scared");
        }
    }

    public void BackToStart()
    {
        transform.position = startPoint.transform.position;
        bestWishes.SetActive(true);
        bestWishes.GetComponent<Fade>().StartFadeOut();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            bumpSoundEffect.pitch = bumpSoundEffectPitch + Random.Range(-0.15f, 0.15f);
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
                Fall();
            }
        }
    }
}
