using UnityEngine;
using System.Collections;

public class PlayerMovement2D_TagBased : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Pulo")]
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public string groundTag = "Ground";
    public float groundCheckRadius = 0.12f;
    public float groundCheckYOffset = 0.05f;

    [Header("Trava de limite (câmera)")]
    public float cameraLeftLimit = -999f;
    public float leftMargin = 0.2f;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;
    private SpriteRenderer sr;

    private float horizontalInput;
    private float currentSpeed;
    private bool jumpRequested;

    private int groundedContacts = 0;
    private bool isGroundedFallback;

    private bool ignoreCameraLimit = false;

    // 🔑 Slow
    private float speedMultiplier = 1f;
    private Coroutine slowRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // ===============================
        // INPUT
        // ===============================
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

        bool isRunning = horizontalInput != 0f;
        bool isRunningFast = isRunning && Input.GetKey(KeyCode.LeftShift);

        currentSpeed = isRunningFast ? runSpeed : walkSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;

        // ===============================
        // FLIP
        // ===============================
        if (horizontalInput != 0)
            sr.flipX = horizontalInput < 0;

        // ===============================
        // ANIMAÇÕES
        // ===============================
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("IsRunningFast", isRunningFast);
    }

    void FixedUpdate()
    {
        // ===============================
        // GROUND CHECK
        // ===============================
        Vector2 footPos = new Vector2(
            transform.position.x,
            col.bounds.min.y - groundCheckYOffset
        );

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            footPos,
            groundCheckRadius
        );

        isGroundedFallback = false;
        foreach (var c in hits)
        {
            if (c != null && !c.isTrigger && c.CompareTag(groundTag))
            {
                isGroundedFallback = true;
                break;
            }
        }

        bool isGrounded = groundedContacts > 0 || isGroundedFallback;
        anim.SetBool("IsGrounded", isGrounded);

        // ===============================
        // MOVIMENTO
        // ===============================
        rb.linearVelocity = new Vector2(
            horizontalInput * currentSpeed * speedMultiplier,
            rb.linearVelocity.y
        );

        // ===============================
        // LIMITE DA CÂMERA (ESQUERDA)
        // ===============================
        if (!ignoreCameraLimit)
        {
            float minX = cameraLeftLimit + leftMargin;

            if (rb.position.x < minX)
            {
                rb.position = new Vector2(minX, rb.position.y);
                rb.linearVelocity = new Vector2(
                    Mathf.Max(0f, rb.linearVelocity.x),
                    rb.linearVelocity.y
                );
            }
        }

        // ===============================
        // PULO
        // ===============================
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            jumpRequested = false;
            groundedContacts = 0;
        }
    }

    // ======================================
    // 🔑 SLOW (USADO POR ENTIDADES)
    // ======================================
    public void ApplySlow(float multiplier, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowRoutine(multiplier, duration));
    }

    IEnumerator SlowRoutine(float multiplier, float duration)
    {
        speedMultiplier = Mathf.Clamp(multiplier, 0.25f, 1f);
        yield return new WaitForSeconds(duration);
        speedMultiplier = 1f;
    }

    // ======================================
    // 🔑 IGNORAR LIMITE (RESPAWN)
    // ======================================
    public void IgnoreCameraLimit(float time)
    {
        StartCoroutine(IgnoreCameraRoutine(time));
    }

    IEnumerator IgnoreCameraRoutine(float time)
    {
        ignoreCameraLimit = true;
        yield return new WaitForSeconds(time);
        ignoreCameraLimit = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;

        foreach (var cp in collision.contacts)
        {
            if (cp.normal.y > 0.6f)
            {
                groundedContacts++;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;
        groundedContacts = Mathf.Max(0, groundedContacts - 1);
    }
}
