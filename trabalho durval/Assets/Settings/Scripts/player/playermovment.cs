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
    public string groundTag = "Ground";     // tag usada no chão
    public float groundCheckRadius = 0.12f;
    public float groundCheckYOffset = 0.05f;

    // componentes
    private Rigidbody2D rb;
    private BoxCollider2D col;

    // input / estado
    private float horizontalInput;
    private float currentSpeed;
    private bool isCrouching;

    // pulo / chão
    private bool jumpRequested = false;
    private int groundedContacts = 0;
    private bool isGroundedFallback = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        if (rb == null) Debug.LogError("[Player] Rigidbody2D faltando!");
        if (col == null) Debug.LogError("[Player] BoxCollider2D faltando!");

        // impede rotação por física
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // --- INPUT ---
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

        // marca pedido de pulo (apenas sinal)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // --- fallback: OverlapCircleAll procurando colliders com tag Ground ---
        Vector2 footPos = new Vector2(transform.position.x, col.bounds.min.y - groundCheckYOffset);
        Collider2D[] hits = Physics2D.OverlapCircleAll(footPos, groundCheckRadius);
        isGroundedFallback = false;
        foreach (var c in hits)
        {
            if (c == null) continue;
            // ignora triggers
            if (c.isTrigger) continue;
            if (c.gameObject.CompareTag(groundTag))
            {
                isGroundedFallback = true;
                break;
            }
        }

        // --- movimento horizontal ---
        rb.linearVelocity = new Vector2(horizontalInput * currentSpeed, rb.linearVelocity.y);

        // --- pulo: só executa se estiver no chão por colisão OU fallback ---
        if (jumpRequested && (IsGrounded() || isGroundedFallback))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;

            // logo após pular, zera contatos para evitar múltiplos pulos
            groundedContacts = 0;
            isGroundedFallback = false;
        }
        else
        {
            // se não pulou, mantemos o pedido até o próximo FixedUpdate (ou até o usuário soltar)
            // caso queira descartar pedidos de pulo ao sair do chão, descomente:
            // if (!IsGrounded() && !isGroundedFallback) jumpRequested = false;
        }
    }

    // retorna true se teve colisões válidas (normal apontando pra cima) com objetos com tag groundTag
    bool IsGrounded()
    {
        return groundedContacts > 0;
    }

    // ------------- colisões -------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;

        foreach (ContactPoint2D cp in collision.contacts)
        {
            // somente conta contatos cujo normal aponta para cima (evita paredes)
            if (cp.normal.y > 0.6f)
            {
                groundedContacts++;
                // Debug.Log("[Player] Colisão com Ground - contacts = " + groundedContacts);
                break;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;

        bool found = false;
        foreach (ContactPoint2D cp in collision.contacts)
        {
            if (cp.normal.y > 0.6f)
            {
                found = true;
                break;
            }
        }

        if (found && groundedContacts == 0)
        {
            groundedContacts = 1; // corrige caso tenha perdido a contagem
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(groundTag)) return;

        if (groundedContacts > 0)
        {
            groundedContacts--;
            // Debug.Log("[Player] Saiu do Ground - contacts = " + groundedContacts);
            if (groundedContacts < 0) groundedContacts = 0;
        }
    }

    void Interagir()
    {
        Debug.Log("Interagiu!");
    }

    // gizmo para ver onde o fallback checa
    void OnDrawGizmosSelected()
    {
        if (col == null) return;
        Gizmos.color = (IsGrounded() || isGroundedFallback) ? Color.green : Color.red;
        Vector2 footPos = new Vector2(transform.position.x, col.bounds.min.y - groundCheckYOffset);
        Gizmos.DrawWireSphere(footPos, groundCheckRadius);

        // mostra estado no editor
        UnityEditor.Handles.Label(transform.position + Vector3.up * 1.2f, $"Ground:{IsGrounded()} FB:{isGroundedFallback}");
    }
}
