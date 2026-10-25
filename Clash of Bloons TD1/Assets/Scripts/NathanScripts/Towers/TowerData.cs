using UnityEngine;

/// <summary>
/// The stats and identity of one tower type, stored as an asset in the Project folders.
///
/// Why a ScriptableObject instead of a constructor:
/// Unity creates MonoBehaviours itself and never calls a constructor with parameters, so values passed that way are inconvenient. 
/// A ScriptableObject is a data file you fill in through the Inspector. 
///
/// Where things are:
/// - TowerData:            Values and parameters (name, damage, ...).
/// - The tower prefab:     Things that belong to the 3D object (its model, body parts, projectile).
///
/// Create one with: right-click in the Project window > Create > Studio Zero > Tower Data.
/// </summary>
[CreateAssetMenu(fileName = "NewTowerData", menuName = "Studio Zero/Tower Data")]
public class TowerData : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Name shown in the shop and the tower info panel.")]
    public string towerName = "New Tower";

    [Tooltip("Short explanation shown to the player.")]
    [TextArea(2, 5)]
    public string description;

    [Header("Placement")]
    [Tooltip("The tower prefab the TowerPlacer spawns when this tower is bought.")]
    public GameObject towerPrefab;

    [Tooltip("Battle Currency cost. Not used yet; ready for when currency is added.")]
    public int cost = 0;

    [Header("Base combat stats")]
    [Tooltip("Damage of one hit, before any Baker bonus.")]
    public float damage = 5f;


    [Tooltip("Seconds between attacks.")]
    public float attackCooldown = 2f;
}
