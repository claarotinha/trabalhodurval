using UnityEngine;

public class JumpCollectable : CollectableBase
{
    public float jumpMultiplier = 1.5f;
    public float duration = 5f;

    protected override void OnCollect(GameObject player)
    {
        if (EffectManager.Instance != null)
            EffectManager.Instance.ActivateJumpBoost(jumpMultiplier, duration);
    }
}
