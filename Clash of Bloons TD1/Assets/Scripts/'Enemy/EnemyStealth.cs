using UnityEngine;

public class EnemyStealth: MonoBehaviour
{
    private bool isStealth = true;
    private Renderer enemyRenderer;
    private Material OriginalMaterial;

    [SerializeField] private Material StealthMaterial;

    private void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        OriginalMaterial = enemyRenderer.material;
        enemyRenderer.material = StealthMaterial;
    }

    public bool IsStealth()
    {
        return isStealth;
    }
    
    public void DisableStealth()
    {
        isStealth = false;
        enemyRenderer.material = OriginalMaterial;
    }
}
