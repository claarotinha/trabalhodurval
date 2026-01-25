using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private List<CollectableBase> collectables = new List<CollectableBase>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterCollectable(CollectableBase collectable)
    {
        if (!collectables.Contains(collectable))
            collectables.Add(collectable);
    }

    // 🔁 Chamado quando o player morre
    public void RespawnCollectables()
    {
        foreach (CollectableBase c in collectables)
        {
            if (c != null)
                c.Respawn();
        }
    }
}
