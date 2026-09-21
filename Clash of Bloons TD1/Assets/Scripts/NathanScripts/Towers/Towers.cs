using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Assets.Scripts.NathanScripts.Towers
{

    /// <summary>
    /// The base class for all towers. Dictates the rules behind all things towers have in common, like a name, attributes, maximum level.
    /// 
    /// This is an abstract class because a Tower should not be able to be instantiated without having a distinct ability.
    /// </summary>
    public abstract class Towers : MonoBehaviour
    {
        private string  _towerType;             // The type of tower. Medieval Hooligan, for example
        private int     _towerCost;             // The amount of Combat currency the tower costs
        private int     _attackPower;           // The amount of damage the tower can deal.
        private float   _attackRange;           // The attack range of the tower.
        private string  _attackType;            // The method of attack the tower uses.
        private bool    _towerMode;             // The switch that activates or deactivates the tower's "mode". Basically turning the unique ability on or off.
        private int     _attackCooldownTimer;   // Dictates the amount of time left before the next attack.
        private int     _attackCooldown;        // Dictates the total amount of time it takes for a tower to attack again.
        private bool    _attackCooldownCheck;   // Checks if the attack is on cooldown.
        private int     _upgradeLevel;          // The current upgrade level of the tower.
        private int     _upgradeLevelLimit;     // The maximum upgrade level of the tower.
        private bool    _isUnlocked;            // The verification on whether or not the tower is allowed to be placed.
        private string  _towerDescription;      // The explanation of the tower's main purpose that is shown to the player.

        /// <summary>
        /// Makes a Tower based on the provided parameters.
        /// </summary>
        /// <param name="towerType">The type of tower. Medieval Hooligan, for example.</param>
        /// <param name="attackPower">The amount of damage the tower can deal.</param>
        /// <param name="attackRange">The attack range of the tower.</param>
        /// <param name="attackType">The method of attack the tower uses.</param>
        /// <param name="attackCooldownTimer">Dictates the amount of time left before the next attack.</param>
        /// <param name="attackCooldown">Dictates the total amount of time it takes for a tower to attack again.</param>
        /// <param name="attackCooldownCheck">Checks if the attack is on cooldown.</param>
        /// <param name="upgradeLevel">The current upgrade level of the tower.</param>
        /// <param name="upgradeLevelLimit">The maximum upgrade level of the tower.</param>
        /// <param name="isUnlocked">The verification on whether or not the tower is allowed to be placed.</param>

        protected Towers(string towerType, int attackPower, float attackRange, string attackType, bool towerMode,
            int attackCooldownTimer, int attackCooldown, bool attackCooldownCheck,
            int upgradeLevel, int upgradeLevelLimit, bool isUnlocked, string towerDescription)
        {
            _towerType = towerType;
            _attackPower = attackPower;
            _attackRange = attackRange;
            _attackType = attackType;
            _towerMode = towerMode;
            _attackCooldownTimer = attackCooldownTimer;
            _attackCooldown = attackCooldown;
            _attackCooldownCheck = attackCooldownCheck;
            _upgradeLevel = upgradeLevel;
            _upgradeLevelLimit = upgradeLevelLimit;
            _isUnlocked = isUnlocked;
            _towerDescription = towerDescription;
        }


        public abstract int TowerRange();
        public abstract string TowerDescription();

        
        
        public virtual void TowerAttack() 
        {
        }
        public virtual void TowerUpgrade() 
        { 
        }

    }
}