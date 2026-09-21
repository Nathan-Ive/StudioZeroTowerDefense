using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// Test class for the Test Tower's ability.
/// 
/// This will have a placeholder ability that will likely be used by the Soldier.
/// </summary>
public class TestAbility : TowerAbilities
{
    public TestAbility(string abilityDescription, string nextDescription) : base(abilityDescription, nextDescription)
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
