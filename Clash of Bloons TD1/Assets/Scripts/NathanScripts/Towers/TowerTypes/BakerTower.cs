using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BAKER: unlocked by the Bakery. A support tower.
///
/// - Attacks like the Soldier (one homing baguette), but much weaker. It uses the standard attack in Tower.
/// - Boosts the direct damage of every other tower inside its range by a flat amount. It can't boost itself.
/// - Two Bakers covering the same tower do not stack: the biggest bonus counts.
/// - Burn damage is not boosted, only direct hits.
/// - Healing comes later, once towers have health.
///
/// How the boost reaches the other towers: Tower.Damage asks BakerTower.GetDamageBonusFor(this) every time
/// a tower fires. The Baker doesn't keep track of who is in range; each tower asks when it needs to know.
///
/// Base stats (set in its TowerData asset): ?? damage, ? second cooldown.
///
/// Planned upgrades (not built yet):
/// [Level 1]   Heals 2.5% every 5 seconds. +2 attack to towers in range.
/// [Level 2]   Heals 5% every 5 seconds.   +3 attack to towers in range.
/// [Level 3]   Heals 10% every 5 seconds.  +4 attack to towers in range.
/// [Max Level] Heals 20% every 5 seconds.  +5 attack to towers in range.
/// Once upgrades exist, the bonus is added before a tower's upgrade multiplier: Soldier Level 2 = (5 + 3) x 1.2.
/// </summary>
public class BakerTower : Tower
{
    // List of every placed Baker. So the bonus check stays organized.
    private static readonly List<BakerTower> PlacedBakers = new List<BakerTower>();

    [Header("Baker")]
    [Tooltip("Flat damage added to every other tower inside this Baker's range (Level 1 = 2).")]
    [SerializeField] private float damageBonus = 2f;

    protected override void OnPlaced()
    {
        PlacedBakers.Add(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        PlacedBakers.Remove(this);
    }

    /// <summary>The damage bonus a tower gets from Bakers.</summary>
    public static float GetDamageBonusFor(Tower tower)
    {
        float bestBonus = 0f;

        foreach (BakerTower baker in PlacedBakers)
        {
            if (baker == null || baker == tower)
            {
                continue; // a Baker never boosts itself
            }

            if (baker.Range.Contains(tower.transform.position))
            {
                bestBonus = Mathf.Max(bestBonus, baker.damageBonus);
            }
        }

        return bestBonus;
    }
}
