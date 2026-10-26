using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct PlacedDownTowers
{
    public string prefabName;
    public Vector3 position;
    public Quaternion rotation;
}

public class TowerSaveData : MonoBehaviour
{
    public static List<PlacedDownTowers> SavedTowers = new List<PlacedDownTowers>();
}