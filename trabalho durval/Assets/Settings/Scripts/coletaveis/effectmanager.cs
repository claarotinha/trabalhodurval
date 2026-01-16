using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    [HideInInspector] public float globalSlowMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ========================
    // SLOW GLOBAL
    // ========================
    public void ActivateSlow(float multiplier, float duration)
    {
        StopCoroutine("SlowRoutine");
        StartCoroutine(SlowRoutine(multiplier, duration));
    }

    private IEnumerator SlowRoutine(float multiplier, float duration)
    {
        globalSlowMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        globalSlowMultiplier = 1f;
    }

    // ========================
    // JUMP BOOST TEMPORÁRIO
    // ========================
    public void ActivateJumpBoost(float multiplier, float duration)
    {
        PlayerMovement2D_TagBased player = FindObjectOfType<PlayerMovement2D_TagBased>();
        if (player != null)
            StartCoroutine(JumpBoostRoutine(player, multiplier, duration));
    }

    private IEnumerator JumpBoostRoutine(PlayerMovement2D_TagBased player, float multiplier, float duration)
    {
        float originalJump = player.jumpForce;
        player.jumpForce *= multiplier;
        yield return new WaitForSeconds(duration);
        player.jumpForce = originalJump;
    }
}
