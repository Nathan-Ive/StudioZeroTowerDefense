using UnityEngine;

/// <summary>
/// The child sphere that defines a tower's attack range.
///
/// How it works:
/// - Unity's built-in Sphere mesh has a radius of 0.5 at scale 1.
///   So the range radius is half of the sphere's X scale: scale 10 = range of 5 units.
///
/// - To make a tower reach further, you scale the sphere in the Scene view.
/// 
/// - The check ignores height (Y). Range is a circle on the ground, so a tall tower and a
///   short enemy still count as "in range" when they stand inside the circle.
/// 
/// - In Play the sphere is hidden. (Later, I will make it so that it only shows while placing or selecting a tower.)
///
/// Setup: 
/// Tower prefab > right-click > 3D Object > Sphere, name it "Range".
/// Add this component, give it a see-through material, and remove its Sphere Collider.
/// Set the Y scale small (0.05) so it looks like a flat circle.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class TowerRange : MonoBehaviour
{
    private MeshRenderer sphereRenderer;

    private void Awake()
    {
        // If someone forgets to remove the collider, turn it off so the sphere never blocks mouse clicks, placement raycasts or projectiles.
        Collider sphereCollider = GetComponent<Collider>();
        if (sphereCollider != null)
        {
            sphereCollider.enabled = false;
        }
    }

    /// <summary>The range in world units, taken from the sphere's size.</summary>
    public float Radius
    {
        get { return transform.lossyScale.x * 0.5f; }
    }

    /// <summary>True when a world position is inside the range circle.</summary>
    public bool Contains(Vector3 worldPosition)
    {
        Vector3 offset = worldPosition - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= Radius * Radius;
    }

    /// <summary>Show or hide the sphere. Hidden does not turn the range off.</summary>
    public void SetVisible(bool visible)
    {
        if (sphereRenderer == null)
        {
            sphereRenderer = GetComponent<MeshRenderer>();
        }
        sphereRenderer.enabled = visible;
    }
}
