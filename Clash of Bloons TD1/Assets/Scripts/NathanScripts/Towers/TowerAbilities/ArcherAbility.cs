using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// The unique ability of the Archer Tower is to split its bullets into three, with a spread shot mode that shoots in a spread pattern, or a homing mode that locks onto an enemy.
/// As it upgrades, it can split its arrows into more arrows. Unlike the Soldier, there is no damage boost.
/// Each bullet that it shoots deals the same amount of damage regardless of upgrade level.
/// 
/// 
/// The base capabilities of the Soldier are: 1 second Attack/Ability cooldown, 3 damage per bullet.
/// 
/// 
/// The upgrades are as follows: 
/// [Level 1]   3 Bullets (Standard) - 2 Bullets (Homing)
/// [Level 2]   5 Bullets (Standard) - 4 Bullets (Homing)
/// [Level 3]   7 Bullets (Standard) - 6 Bullets (Homing)
/// [Max Level] 10 Bullets (Standard) - 8 Bullets (Homing)
/// 
/// </summary>
public class ArcherAbility : TowerAbilities
{
    public ArcherAbility(string abilityDescription, string nextDescription) : base(abilityDescription, nextDescription)
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
    }
}
