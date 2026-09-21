using UnityEngine;


namespace Assets.Scripts.NathanScripts.Towers
{

    /// <summary>
    /// The base class for all abilities for every tower.
    /// 
    /// This is an abstract class because there is no generic ability. Similar to the towers themselves, each ability belonging to each tower is unique.
    /// So, the funcctionality and description of the tower is unique each time. 
    /// For example, the amount of ugprades left, the current ability description for the current upgrade level, etc.
    /// </summary>
    public abstract class TowerAbilities : MonoBehaviour
    {
        
        private string  _abilityDescription; // The explanation of what the ability does.
        private string  _nextDescription;    // The explanation of what the ability of the next upgrade level does.
                                             // (Make the default value one that states there is no next level to the ability)



        /// <summary>
        /// The unique ability belonging to a tower, as well as its unique capability, if it has one.
        /// </summary>
        /// <param name="abilityDescription">The explanation of what a tower's current ability does.</param>
        /// <param name="nextDescription">The explanation of what a tower's next ability does, after upgrading.</param>
        protected TowerAbilities(string abilityDescription, string nextDescription)
        {
            _abilityDescription = abilityDescription;
            _nextDescription = nextDescription;
        }

        public abstract void TowerAbility();
        public abstract string CurrentAbilityDesc();
        public abstract string NextAbilityDesc();
    }
}