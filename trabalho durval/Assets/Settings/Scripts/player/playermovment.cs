using UnityEngine;
using System.Collections;

public class PlayerMovement2D_TagBased : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

    [Header("Pulo")]
    public float jumpForce = 7f;

    [Header("Ataque (Bola de Fogo)")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireballForce = 8f;
    public float attackCooldown = 0.4f;

    [Header("Ground Check")]
    public string groundTag = "Ground";
    public float groundCheckRadius = 0.12f;
    public float groundCheckYOffset = 0.05f;

    [Header("Trava da câmera (esquerda)")]
    public float cameraLeftLimit = -999f;
    public float leftMargin = 0.2f;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private Animator anim;
    private SpriteRenderer sr;

    private float horizontalInput;
    private float currentSpeed;
    private bool jumpRequested;
    private bool isGrounded;
    private bool canAttack = true;

    private bool ignoreCameraLimit = false;

    // Slow
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
        // INPUT MOVIMENTO
        // ===============================
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

        bool isRunning = horizontalInput != 0f;
        bool isRunningFast = isRunning && Input.GetKey(KeyCode.LeftShift);

        currentSpeed = isRunningFast ? runSpeed : walkSpeed;

        // ===============================
        // INPUT PULO
        // ===============================
        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;

        // ===============================
        // INPUT ATAQUE
        // ===============================
        if (Input.GetMouseButtonDown(0) && canAttack)
            StartCoroutine(Attack());

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
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsJumping", !isGrounded);
        anim.SetFloat("VerticalSpeed", rb.linearVelocity.y);
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

        isGrounded = false;
        foreach (var c in hits)
        {
            if (c != null && !c.isTrigger && c.CompareTag(groundTag))
            {
                isGrounded = true;
                break;
            }
        }

        // ===============================
        // MOVIMENTO
        // ===============================
        rb.linearVelocity = new Vector2(
            horizontalInput * currentSpeed * speedMultiplier,
            rb.linearVelocity.y
        );

        // ===============================
        // LIMITE DA CÂMERA
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    // ======================================
    // 🔥 ATAQUE (BOLA DE FOGO)
    // ======================================
    IEnumerator Attack()
    {
        canAttack = false;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.1f);

        GameObject fireball = Instantiate(
            fireballPrefab,
            firePoint.position,
            Quaternion.identity
        );

        Rigidbody2D fireRb = fireball.GetComponent<Rigidbody2D>();
        float dir = sr.flipX ? -1f : 1f;
        fireRb.linearVelocity = new Vector2(dir * fireballForce, 0f);

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // ======================================
    // 🔑 IGNORAR LIMITE DA CÂMERA (RESPAWN)
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

    // ======================================
    // 🔑 SLOW
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
}
