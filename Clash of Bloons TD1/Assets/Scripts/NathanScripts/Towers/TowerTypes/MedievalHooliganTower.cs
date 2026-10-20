using UnityEngine;

/// <summary>
/// MEDIEVAL HOOLIGAN: unlocked by the Campsite. A support tower that slows enemies with dirt.
///
/// Two things it does:
/// 1. Normal attack (every few seconds): throws a clump of dirt at one enemy. Small damage,
///    and the enemy is slowed for a short time.
/// 2. Ability (every 15 seconds): drops a patch of dirt on the path where its target is standing.
///    Every enemy that walks through is slowed. The patch disappears after a set number of enemies.
///    Only one patch at a time, and the 15 second wait starts when a patch is thrown.
///
/// Base stats (set in its TowerData asset): ? damage, ? second cooldown.
/// A 50% enemy movement speed decrease for 2 seconds.
///
/// Planned upgrades for the patch (not built yet; the fields below already support them):
/// [Level 1]   Slows (no damage)                                      - gone after 10 enemies
/// [Level 2]   Slows + deals damage                                   - gone after 15 enemies
/// [Max Level] Stops for 2 s + permanently slows + deals damage       - gone after 20 enemies
///             (permanent = set Patch Slow Duration to a huge number, e.g. 9999)
/// </summary>
public class MedievalHooliganTower : Tower
{
}
