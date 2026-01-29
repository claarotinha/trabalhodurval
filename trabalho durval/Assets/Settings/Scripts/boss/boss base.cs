using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Rodadas")]
    public int maxRounds = 3;
    private int currentRound = 1;

    [Header("Tempo")]
    public float roundDuration = 10f;
    public float restDuration = 3f;

    [Header("Projéteis")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 5f;
    public float shootInterval = 1.5f;

    [Header("Pisada")]
    public GameObject stompAreaPrefab;
    public Transform stompPoint;
    public float stompInterval = 4f;
    public int stompDamage = 1;

    private bool isFighting;
    private BossMovement movement;

    void Start()
    {
        movement = GetComponent<BossMovement>();
        StartCoroutine(RoundLoop());
    }

    IEnumerator RoundLoop()
    {
        while (currentRound <= maxRounds)
        {
            Debug.Log("Rodada " + currentRound);

            SetupMovementForRound();
            isFighting = true;

            StartCoroutine(ShootRoutine());
            StartCoroutine(StompRoutine());

            yield return new WaitForSeconds(roundDuration);

            isFighting = false;
            yield return new WaitForSeconds(restDuration);

            IncreaseDifficulty();
            currentRound++;
        }

        DefeatBoss();
    }

    IEnumerator ShootRoutine()
    {
        while (isFighting)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }

    IEnumerator StompRoutine()
    {
        while (isFighting)
        {
            Stomp();
            yield return new WaitForSeconds(stompInterval);
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.left * projectileSpeed;
    }

    void Stomp()
    {
        GameObject area = Instantiate(stompAreaPrefab, stompPoint.position, Quaternion.identity);
        BossStompArea stomp = area.GetComponent<BossStompArea>();

        if (stomp != null)
            stomp.damage = stompDamage;
    }

    void SetupMovementForRound()
    {
        if (movement == null) return;

        switch (currentRound)
        {
            case 1:
                movement.SetPattern(BossMovement.MovementPattern.Hover);
                break;

            case 2:
                movement.SetPattern(BossMovement.MovementPattern.SideToSide);
                break;

            case 3:
                movement.SetPattern(BossMovement.MovementPattern.Jumping);
                break;
        }
    }

    void IncreaseDifficulty()
    {
        projectileSpeed += 2f;
        shootInterval = Mathf.Max(0.5f, shootInterval - 0.3f);
        stompInterval = Mathf.Max(2f, stompInterval - 0.5f);
        stompDamage++;

        if (movement != null)
            movement.IncreaseDifficulty(0.8f);
    }

    void DefeatBoss()
    {
        Debug.Log("Boss derrotado");
        Destroy(gameObject);
    }
}
