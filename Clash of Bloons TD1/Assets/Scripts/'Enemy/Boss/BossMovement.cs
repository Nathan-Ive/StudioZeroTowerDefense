using UnityEngine;

public class BossMovement : EnemyMovement
{
    [Header("Boss Speedsettings")]
    [Tooltip("start speed settings.")]
    public float baseSpeed = 3f;

    [Tooltip("max speed")]
    public float maxSpeed = 8f;

    [Header("Debug / Info")]
    [SerializeField] private float initialTotalDistance = 0f;

    protected override void Start()
    {
        base.Start();
        initialTotalDistance = DistanceToEnd;
    }

    protected override void Update()
    {
        if (initialTotalDistance > 0f)
        {
            float progress = 1f - Mathf.Clamp01(DistanceToEnd / initialTotalDistance);
            speed = Mathf.Lerp(baseSpeed, maxSpeed, progress);
        }
        base.Update();
    }
}