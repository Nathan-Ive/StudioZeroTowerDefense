using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveTower()
    {
        TowerSaveData.SavedTowers.Clear();
        GameObject[] builtTowers = GameObject.FindGameObjectsWithTag("BuildTower");

        foreach (GameObject tower in builtTowers)
        {
            BuildingInfo info = tower.GetComponent<BuildingInfo>();
            if (info != null)
            {
                PlacedDownTowers savedTower = new PlacedDownTowers
                {
                    prefabName = info.prefabName,
                    position = tower.transform.position,
                    rotation = tower.transform.rotation
                };

                TowerSaveData.SavedTowers.Add(savedTower);
            }
        }

        Debug.Log("Towers saved Number: " + TowerSaveData.SavedTowers.Count);
    }
}