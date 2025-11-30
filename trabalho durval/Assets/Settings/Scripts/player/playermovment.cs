using UnityEngine;

public class PlayerMovement2D_TagBased : MonoBehaviour
{
    [Header("Velocidades")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float crouchSpeed = 2.5f;

    [Header("Pulo")]
    public float jumpForce = 7f;

    [Header("Ground Check fallback")]
    public string groundTag = "Ground";     
    public float groundCheckRadius = 0.12f;
    public float groundCheckYOffset = 0.05f;

    [Header("Limite Esquerdo")]
    public float minPlayerX = 0f;   // <--- ADICIONADO

    private Rigidbody2D rb;
    private BoxCollider2D col;

    private float horizontalInput;
    private float currentSpeed;
    private bool isCrouching;

    private bool jumpRequested = false;
    private int groundedContacts = 0;
    private bool isGroundedFallback = false;

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

        if (Input.GetKey(KeyCode.F))
        {
            isCrouching = true;
            currentSpeed = crouchSpeed;
        }
        else isCrouching = false;

        if (Input.GetKeyDown(KeyCode.E)) Interagir();
        if (Input.GetKeyDown(KeyCode.Space)) jumpRequested = true;
    }

    void FixedUpdate()
    {
        Vector2 footPos = new Vector2(transform.position.x, col.bounds.min.y - groundCheckYOffset);
        Collider2D[] hits = Physics2D.OverlapCircleAll(footPos, groundCheckRadius);

        isGroundedFallback = false;
        foreach (var c in hits)
        {
            if (c == null || c.isTrigger) continue;
            if (c.gameObject.CompareTag(groundTag))
            {
                isGroundedFallback = true;
                break;
            }
        }

        // MOVIMENTO + TRAVA NA ESQUERDA ------------------------------------
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);

        // impede o player de passar do limite mínimo
        if (transform.position.x < minPlayerX)
        {
            transform.position = new Vector3(minPlayerX, transform.position.y, transform.position.z);
        }
        //-------------------------------------------------------------------

        if (jumpRequested && (IsGrounded() || isGroundedFallback))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;

            groundedContacts = 0;
            isGroundedFallback = false;
        }
    }

    bool IsGrounded()
    {
        return groundedContacts > 0;
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;

        bool found = false;
        foreach (var cp in collision.contacts)
        {
            if (cp.normal.y > 0.6f)
            {
                found = true;
                break;
            }
        }

        if (found && groundedContacts == 0)
            groundedContacts = 1;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;
        groundedContacts--;
        if (groundedContacts < 0) groundedContacts = 0;
    }

    void Interagir()
    {
        Debug.Log("Interagiu!");
    }
}

