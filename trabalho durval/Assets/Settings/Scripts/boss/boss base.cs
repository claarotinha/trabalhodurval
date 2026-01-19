using UnityEngine;
using System.Collections;

public class BossAttackPattern : MonoBehaviour
{
    [Header("Ataques")]
    public GameObject projectilePrefab;      // Projétil único
    public Transform shootPoint;             // De onde sai o projétil

    public GameObject areaAttackPrefab;      // Ataque em área
    public Transform[] areaPoints;           // Pontos onde o ataque em área aparece

    [Header("Rodadas")]
    public float attackIntervalEasy = 3f;
    public float attackIntervalMedium = 2f;
    public float attackIntervalHard = 1f;

    private int currentRound = 0;            // 0 = fácil, 1 = médio, 2 = difícil
    private bool isPaused = false;           // Pausa para interação
    private bool canInteract = false;

    void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    // Rotina das rodadas
    IEnumerator RoundRoutine()
    {
        while (currentRound < 3)
        {
            float interval = GetAttackInterval();
            float roundDuration = 10f; // duração de cada rodada (ajuste como quiser)

            float timer = 0f;
            while (timer < roundDuration)
            {
                if (!isPaused) DoAttack();
                timer += interval;
                yield return new WaitForSeconds(interval);
            }

            // Pausa para interação
            isPaused = true;
            canInteract = true;
            Debug.Log("Boss enfraquecido! Pressione E para interagir.");
            float pauseTime = 5f; // tempo da pausa
            yield return new WaitForSeconds(pauseTime);
            canInteract = false;
            isPaused = false;

            currentRound++;
        }

        Debug.Log("Boss derrotado!");
    }

    // Determina intervalo de ataque de acordo com a rodada
    float GetAttackInterval()
    {
        switch (currentRound)
        {
            case 0: return attackIntervalEasy;
            case 1: return attackIntervalMedium;
            case 2: return attackIntervalHard;
            default: return 2f;
        }
    }

    // Escolhe ataque aleatório
    void DoAttack()
    {
        int choice = Random.Range(0, 2); // 0 = projétil único, 1 = ataque área
        if (choice == 0) ShootSingle();
        else ShootArea();
    }

    // Ataque projétil único
    void ShootSingle()
    {
        if (!projectilePrefab || !shootPoint) return;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.left * 5f; // velocidade padrão, ajuste se quiser
        }
    }

    // Ataque em área
    void ShootArea()
    {
        if (!areaAttackPrefab || areaPoints.Length == 0) return;

        foreach (Transform point in areaPoints)
        {
            Instantiate(areaAttackPrefab, point.position, Quaternion.identity);
        }
    }

    // Chamado pelo player quando interage com E
    public void Interact()
    {
        if (canInteract)
        {
            Debug.Log("Player interagiu com o boss!");
            // Aqui você pode colocar efeitos como diálogo ou reduzir vida do boss
        }
    }
}
