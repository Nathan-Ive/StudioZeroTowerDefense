using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// The unique ability of the Baker Tower is supportive. The Baker recovers nearby Tower health for the ones that have been damaged and buffs direct damage.
/// The Baker heals by percentage, and can only heal within its range.
/// Offensively the Baker functions the same as the Soldier, but signficantly weaker.
/// The Baker cannot heal itself, and cannot buff itself.
/// The Baker buffs damage by a flat amount, not a percentage. 
/// This means that Towers like the Soldier or Wizard get a percentage boost, since its upgrade level boosts based on its current base-attack.
/// Towers like the Archer and Medieval Hooligan just get flat offensive boosts with no percentages, due to their damage percentage not increasing with their own upgrades.
/// 
/// For example, a Soldier has an attack stat of 5. At level 2 that becomes (5 x 1.2 = 6).
/// The Baker boosts a Soldier's attack, turning the formula into (8 x 1.2 = 10). This means the soldier gets an attack boost of 4 instead of 3.
/// A max level Soldier would have a formula that looks more llike this (5 x 2 = 10).
/// Getting the Baker's  attack buff, it would become (8 x 2 = 16). Basically, the offensive boost gets increased based on the multiplier.
/// 
/// 
/// The base capabilities of the Baker are: 5 second Attack/Ability cooldown, 2 damage per bullet.
/// 
/// 
/// The upgrades are as follows:
/// [Level 1] Heals 2.5% of health every 5 seconds. +2 attack to Towers in range.
/// [Level 2] Heals 5% of health every 5 seconds. +3 attack to Towers in range.
/// [Level 3] Heals 10% of health every 5 seconds. +4 attack to Towers in range.
/// [Max Level] Heals 20% of health every 5 seconds. +5 attack to Towers in range.
/// 
/// </summary>
public class BakerAbility : TowerAbilities
{
    public BakerAbility(string abilityDescription, string nextDescription) : base(abilityDescription, nextDescription)
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
