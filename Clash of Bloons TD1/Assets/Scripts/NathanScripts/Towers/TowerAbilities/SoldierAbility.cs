using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// The Soldier Tower is the starting Tower and does the most basic offensive tactic out of any tower. 
/// Throwing individual bullets that increase in damage, and decrease in cooldown, both based on upgrade level, towards the enemy.
/// 
/// 
/// The base capabilities of the Soldier are: 2 second Attack/Ability cooldown, 5 damage per attack.
/// 
/// 
/// The upgrades are as follows:
/// [Level 1]   100% attack & 100% cooldown   (5 x 1.0 = 5 Damage) (2,0 Seconds)
/// [Level 2]   120% attack & 80% cooldown    (5 x 1.2 = 6 Damage) (1,6 seconds)
/// [Level 3]   140% attack & 60% cooldown    (5 x 1.4 = 7 Damage) (1,2 seconds)
/// [Level 4]   160% attack & 40% cooldown    (5 x 1.6 = 8 Damage) (0,8 seconds)
/// [Max Level] 200% attack & 20% cooldown    (5 x 2.0 = 10 Damage) (0,4 seconds)
/// 
/// </summary>
public class SoldierAbility : TowerAbilities
{
    public SoldierAbility(string abilityDescription, string nextDescription) : base(abilityDescription, nextDescription)
    {
    }

    public override string CurrentAbilityDesc()
    {
        throw new System.NotImplementedException();
    }

    public override string NextAbilityDesc()
    {
        throw new System.NotImplementedException();
    }

    public override void TowerAbility()
    {
        throw new System.NotImplementedException();
    }
}
