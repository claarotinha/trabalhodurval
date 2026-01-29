using UnityEngine;
using System.Collections;

public class BossMovement : MonoBehaviour
{
    public enum MovementPattern
    {
        Hover,
        SideToSide,
        Jumping
    }

    [Header("Movimento")]
    public MovementPattern currentPattern;
    public float moveSpeed = 2f;
    public float moveHeight = 1.5f;
    public float moveWidth = 3f;

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        switch (currentPattern)
        {
            case MovementPattern.Hover:
                Hover();
                break;

            case MovementPattern.SideToSide:
                SideToSide();
                break;

            case MovementPattern.Jumping:
                Jumping();
                break;
        }
    }

    void Hover()
    {
        float y = Mathf.Sin(timer * moveSpeed) * moveHeight;
        transform.position = startPos + new Vector3(0f, y, 0f);
    }

    void SideToSide()
    {
        float x = Mathf.Sin(timer * moveSpeed) * moveWidth;
        transform.position = startPos + new Vector3(x, 0f, 0f);
    }

    void Jumping()
    {
        float y = Mathf.Abs(Mathf.Sin(timer * moveSpeed)) * moveHeight;
        transform.position = startPos + new Vector3(0f, y, 0f);
    }

    // 🔑 chamado pelo controller
    public void IncreaseDifficulty(float speedBonus)
    {
        moveSpeed += speedBonus;
    }

    public void SetPattern(MovementPattern pattern)
    {
        currentPattern = pattern;
        timer = 0f;
        startPos = transform.position;
    }
}
