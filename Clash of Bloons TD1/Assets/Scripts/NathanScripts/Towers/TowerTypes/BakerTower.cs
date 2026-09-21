using Assets.Scripts.NathanScripts.Towers;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class BakerTower : Towers
{
    public BakerTower(string towerType, int attackPower, float attackRange, string attackType, bool towerMode, int attackCooldownTimer, int attackCooldown, bool attackCooldownCheck, int upgradeLevel, int upgradeLevelLimit, bool isUnlocked, string towerDescription) : base(towerType, attackPower, attackRange, attackType, towerMode, attackCooldownTimer, attackCooldown, attackCooldownCheck, upgradeLevel, upgradeLevelLimit, isUnlocked, towerDescription)
    {
    }

    public override void TowerAttack()
    {
        base.TowerAttack();
    }


    public override int TowerRange()
    {
        throw new System.NotImplementedException();
    }

    public override string TowerDescription()
    {
        throw new System.NotImplementedException();
    }
}
