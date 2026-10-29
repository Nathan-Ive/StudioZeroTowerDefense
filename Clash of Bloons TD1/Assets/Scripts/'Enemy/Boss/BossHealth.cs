using UnityEngine;

public class BossHealth : Health
{
    protected override void Die()
    {
        if (BossUIManager.Instance != null)
        {
            BossUIManager.Instance.ShowWinScreen();
        }

        base.Die();
    }
}