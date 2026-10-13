using UnityEngine;

/// <summary>
/// The stats and identity of one tower type, stored as an asset in the Project window.
///
/// Why a ScriptableObject instead of a constructor:
/// Unity creates MonoBehaviours itself and never calls a constructor with parameters, so values passed that way are inconvenient. 
/// A ScriptableObject is a data file you fill in through the Inspector. 
///
/// What lives where:
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

    [Header("Base combat stats")]
    [Tooltip("Damage of one hit, before any Baker bonus.")]
    public float damage = 5f;
}
