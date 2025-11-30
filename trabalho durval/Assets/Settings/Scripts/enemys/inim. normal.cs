using UnityEngine;

public class ShadowEnemyNormal : ShadowEnemyBase
{
    public Sprite normalSprite;

    protected override void Start()
    {
        base.Start();
        if (sr != null && normalSprite != null) sr.sprite = normalSprite;
        speed = 3f; // velocidade normal
    }
}
