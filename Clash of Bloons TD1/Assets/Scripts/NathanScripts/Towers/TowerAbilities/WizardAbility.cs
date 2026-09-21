using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// The unique ability of the Wizard tower is to throw fireballs that pierce through enemies and deal burn-damage over time as it upgrades.
/// Similar to the Soldier, it boosts its damage with each upgrade, but not nearly to the same extent. The DoT damage never increases. Only direct damage gets upgraded.
/// (DoT stands for Damage over Time)
/// 
/// 
/// The base capabilities of the Wizard are: 2.5 second Attack/Ability cooldown, 4 damage per attack.
/// 
/// 
/// The upgrades are as follows:
/// [Level 1] Fireball pierces through 3 enemies, 0 seconds of DoT, 100% damage ()
/// [Level 2] Fireball pierces through 5 enemies, 2 seconds of DoT, 110% damage
/// [Level 3] Fireball pierces through 9 enemies, 4 seconds of DoT, 125% damage
/// [Max Level] Fireball pierces through infinite enemies, 8 seconds of DoT, 150% damage
/// 
/// </summary>
public class WizardAbility : TowerAbilities
{
    public WizardAbility(string abilityDescription, string nextDescription) : base(abilityDescription, nextDescription)
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
