using UnityEngine;

public class ShadowEnemyObservadora : ShadowEnemyBase
{
    public Sprite observadoraSprite;

    protected override void Start()
    {
        base.Start();
        if (sr != null && observadoraSprite != null) sr.sprite = observadoraSprite;
        speed = 0f; // não se move
    }

    protected override void Move()
    {
        // Observadora não se move, permanece onde spawnou
    }
}
