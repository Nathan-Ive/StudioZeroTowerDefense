using UnityEngine;

public class TowerTypes
{
    public enum TowerType
    {
        Soldier, //The standard tower, throws his sword at the enemy in a straight line.
        Archer,  //Multi-hit tower, shoots arrows in a spread pattern.
        Wizard, //Damage over time tower, throws a fireball that passes through enemies and applies the burn status to an enemy, dealing a certain amount of damage per second.
        Baker, //Support tower, increases the direct damage other towers around it deal, throws the slowest projectile. (Does not increase Damage over time damage). 
        MedievalHooligan //Support tower, throws dirt on the ground that slows the enemy down. Attacks directly very infrequently, when he does, his projectile fully stops the enemy for 2 seconds while they clean themselves.
    }
}
