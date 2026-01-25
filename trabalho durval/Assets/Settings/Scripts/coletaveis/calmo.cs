using UnityEngine;

public class SlowCollectable : CollectableBase
{
    [Range(0.1f, 1f)]
    public float slowMultiplier = 0.5f;
    public float duration = 5f;

    protected override void OnCollect(GameObject player)
    {
        if (EffectManager.Instance != null)
            EffectManager.Instance.ActivateSlow(slowMultiplier, duration);
    }
}
