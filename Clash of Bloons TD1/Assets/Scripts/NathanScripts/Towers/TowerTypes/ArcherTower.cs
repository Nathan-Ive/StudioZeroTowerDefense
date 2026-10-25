/// <summary>
/// SOLDIER: the starting tower, unlocked by the Main Building.
/// Throws a sword at one enemy at a time. The most basic offensive tower.
///
/// It overrides nothing: the standard attack in Tower is the Soldier attack.
///
/// Base stats (set in its TowerData asset): 5 damage, 2 second cooldown.
///
/// Planned upgrades (not built yet):
/// [Level 1]   100% attack & 100% cooldown   (5 x 1.0 = 5 Damage)  (2.0 seconds)
/// [Level 2]   120% attack & 80% cooldown    (5 x 1.2 = 6 Damage)  (1.6 seconds)
/// [Level 3]   140% attack & 60% cooldown    (5 x 1.4 = 7 Damage)  (1.2 seconds)
/// [Level 4]   160% attack & 40% cooldown    (5 x 1.6 = 8 Damage)  (0.8 seconds)
/// [Max Level] 200% attack & 20% cooldown    (5 x 2.0 = 10 Damage) (0.4 seconds)
/// </summary>
public class ArcherTower : Tower
{
}
