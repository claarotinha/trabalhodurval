using UnityEngine;

public class ShadowEnemyRapido : ShadowEnemyBase
{
    public Sprite rapidoSprite;

    protected override void Start()
    {
        base.Start();
        if (sr != null && rapidoSprite != null) sr.sprite = rapidoSprite;
        speed = 5f; // mais rápido que o normal
    }
}
