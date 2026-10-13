using UnityEngine;



/// <summary>
/// The script that manages the Combat Scene. Checks which Combat Stage should be initiated, and checks whether or not the player wants the scene to end or not.
/// 
/// 
/// This Scene Manager can: 
///     Start the Combat Loop, 
///     Pause the Combat Loop,
///     End the Combat Loop,
///     Send the player to the Town Scene,
///     Send the player to the next Combat Stage in the Combat Loop,
///     Add onto the total amount of Town Currency after a battle is completed,
///     Use the functionalities associated with Combat.
///     
/// This Scene Manager cannot:
///     Close the game,
///     Use the functionalities associated with Town Building.
/// 
/// 
/// The Combat Loop is:
///     Place Towers --> Defeat Enemies --> Get Money --> Place more Towers --> Beat final Wave --> Beat Boss --> Win --> Go to next stage or Go Back to Town.
///     Or Fail to defeat enemies --> Lose --> Restart stage or Go back to Town
/// </summary>
public class CombatSceneManager
{
    
}
