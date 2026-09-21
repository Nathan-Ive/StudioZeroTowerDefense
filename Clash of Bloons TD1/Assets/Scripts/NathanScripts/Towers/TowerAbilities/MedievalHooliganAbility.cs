using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// The unique ability of the Medieval Hooligan Tower is to throw dirt on the ground that slows enemies down.
/// The dirt on the ground disappears after a certain amount of enemies step on it.
/// Only one patch of dirt can exist at a time.
/// Once thrown down, the Medieval Hooligan cannot throw another patch of dirt on the ground for 15 seconds, regardless of upgrade level.
/// The Medieval Hooligan also attacks normally, by throwing dirt at the enemy. This dirt slows down the enemy, but can only target one at a time.
/// The normal dirt is also unaffected by the upgrade level, so it will only ever slow enemies down temporarily.
/// As the Medieval Hooligan is upgraded, the dirt it throws on the floor gains more effects.
/// 
/// 
/// The base capabilities of the Medieval Hooligan are: 5 second Attack cooldown, 15 second Ability cooldown, 1 damage per attack.
/// 
/// 
/// The upgrades are as follows: 
/// [Level 1]   Temporarily Slows down enemies (No damage) - Disappears after 10 enemies walk into it. 
/// [Level 2]   Temporarily Slows down enemies + Deals Damage - Disappears after 15 enemies walk into it.
/// [Max Level] Temporarily Stops enemies for 2 seconds + Permanently Slows down enemies + Deals Damage - Disappears after 20 enemies walk into it.
/// 
/// </summary>
public class MedievalHooliganAbility : TowerAbilities
{
    public MedievalHooliganAbility(string abilityDescription, string nextDescription) : base(abilityDescription, nextDescription)
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
