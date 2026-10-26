using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    void Start()
    {
        RebuildTower();
    }

    public void RebuildTower()
    {
        foreach (var data in TowerSaveData.SavedTowers)
        {
            GameObject prefab = Resources.Load<GameObject>(data.prefabName);
            if (prefab != null)
            {
                GameObject newTower = Instantiate(prefab, data.position, data.rotation);
                newTower.tag = "BuildTower";
            }
            else
            {
                Debug.LogWarning("Kon prefab niet vinden in Resources: " + data.prefabName);
            }
        }
    }
}