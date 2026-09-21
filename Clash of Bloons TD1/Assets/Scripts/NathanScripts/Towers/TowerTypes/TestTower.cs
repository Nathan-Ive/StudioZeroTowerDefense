using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// The Test Tower. This tower won't be purchasable by the player.
/// 
/// I made this purely to test compatibility with other things and features, before making sweeping changes to the other tower scripts.
/// 
/// Before implementing something in another tower, be sure to try it out here. Or don't. I'm not looking.
/// </summary>
public class TestTower : Towers
{
    public TestTower(string towerType, int attackPower, float attackRange, string attackType, bool towerMode, int attackCooldownTimer, int attackCooldown, bool attackCooldownCheck, int upgradeLevel, int upgradeLevelLimit, bool isUnlocked, string towerDescription) : base(towerType, attackPower, attackRange, attackType, towerMode, attackCooldownTimer, attackCooldown, attackCooldownCheck, upgradeLevel, upgradeLevelLimit, isUnlocked, towerDescription)
    {
    }

    public override string TowerDescription()
    {
        throw new System.NotImplementedException();
    }

    public override int TowerRange()
    {
        throw new System.NotImplementedException();
    }
}
