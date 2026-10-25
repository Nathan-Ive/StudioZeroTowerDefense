using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A projectile that can also fly in a straight line, instead of following one enemy.
/// It inherits everything from Projectile (speed, damage, homing, hitting) and only adds the straight flight.
///
/// Used by the Archer:
///   Spread mode fires these in a straight fan with LaunchInDirection().
///   Homing mode uses LaunchAt(), the homing it inherited from Projectile.
///
/// A straight projectile hits the first enemy it passes through and then disappears.
/// The distance check ignores height, so it still hits a short monkey it flies over.
///
/// Setup: use this component on the projectile prefab instead of Projectile.
/// </summary>
public class StraightProjectile : Projectile
{
    // How many enemies it can still hit before it disappears. FireballProjectile uses this to pierce.
    protected int hitsLeft = 1;

    private bool flyingStraight;
    private Vector3 direction;
    private Vector3 startPosition;
    private float maxTravelDistance;

    // Enemies already hit, so one projectile never hits the same monkey twice.
    private readonly HashSet<EnemyMovement> alreadyHit = new HashSet<EnemyMovement>();

    /// <summary>Straight: fly in this direction until it has hit enough enemies or travelled far enough.</summary>
    public void LaunchInDirection(Vector3 newDirection, float newDamage, float travelDistance)
    {
        flyingStraight = true;
        direction = newDirection.normalized;
        damage = newDamage;
        maxTravelDistance = travelDistance;
        startPosition = transform.position;

        if (direction.sqrMagnitude > 0f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        lifeTimer = 0f;
        launched = true;
    }

    protected override void Fly()
    {
        // Launched with LaunchAt(): fly like a normal homing projectile.
        if (!flyingStraight)
        {
            base.Fly();
            return;
        }

        transform.position += direction * speed * Time.deltaTime;

        // Loop backwards, so the list can safely change while looping.
        List<EnemyMovement> enemies = EnemyMovement.ActiveEnemies;
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            EnemyMovement enemy = enemies[i];
            if (enemy == null || alreadyHit.Contains(enemy))
            {
                continue;
            }

            if (FlatDistance(transform.position, enemy.transform.position) <= hitRadius)
            {
                alreadyHit.Add(enemy);
                HitEnemy(enemy);

                hitsLeft--;
                if (hitsLeft <= 0)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }

        // Missed everything and flew out of range.
        if (FlatDistance(startPosition, transform.position) >= maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    private static float FlatDistance(Vector3 a, Vector3 b)
    {
        a.y = 0f;
        b.y = 0f;
        return Vector3.Distance(a, b);
    }
}
