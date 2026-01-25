using UnityEngine;
using System.Collections;

public class PlayerMovement2D_TagBased : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;

    [Header("Pulo")]
    public float jumpForce = 7f;

    [Header("Ground Check")]
    public string groundTag = "Ground";
    public float groundCheckRadius = 0.12f;
    public float groundCheckYOffset = 0.05f;

    [Header("Trava de limite (câmera)")]
    public float cameraLeftLimit = -999f;
    public float leftMargin = 2f;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private float horizontalInput;
    private float currentSpeed;
    private bool jumpRequested;

    private int groundedContacts = 0;
    private bool isGroundedFallback = false;

    // 🔑 CONTROLE DE RESPAWN
    private bool ignoreCameraLimit = false;

    // 🔑 MODIFICADOR DE VELOCIDADE (lentidão segura)
    private float speedMultiplier = 1f;
    private Coroutine slowRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        horizontalInput = 0f;

        if (Input.GetKey(KeyCode.A)) horizontalInput = -1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput = 1f;

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;
    }

    void FixedUpdate()
    {
        // ===============================
        // GROUND CHECK (SEM OBJETO VAZIO)
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

        // ===============================
        // MOVIMENTO (COM LENTIDÃO SEGURA)
        // ===============================
        rb.linearVelocity = new Vector2(
            horizontalInput * currentSpeed * speedMultiplier,
            rb.linearVelocity.y
        );

        // ===============================
        // TRAVA DA CÂMERA
        // ===============================
        if (!ignoreCameraLimit)
        {
            float minAllowedX = cameraLeftLimit - leftMargin;
            if (transform.position.x < minAllowedX)
            {
                transform.position = new Vector3(
                    minAllowedX,
                    transform.position.y,
                    transform.position.z
                );
            }
        }

        // ===============================
        // PULO
        // ===============================
        if (jumpRequested && (groundedContacts > 0 || isGroundedFallback))
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
    // 🔑 LENTIDÃO (USADA PELA OBSERVADORA)
    // ======================================
    public void ApplySlow(float multiplier, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(SlowRoutine(multiplier, duration));
    }

    IEnumerator SlowRoutine(float multiplier, float duration)
    {
        // impede travamento total
        speedMultiplier = Mathf.Clamp(multiplier, 0.25f, 1f);

        yield return new WaitForSeconds(duration);

        speedMultiplier = 1f;
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
