/// <summary>
/// Which enemy a tower picks when more than one is inside its range.
///
/// - First:   the enemy closest to the end of the path (the most dangerous one).
/// - Last:    the enemy furthest from the end of the path (the least dangerous one).
/// - Closest: the enemy closest to the tower itself.
///
/// "First" and "Last" use EnemyMovement.DistanceToEnd, which measures the distance still left to walk along the waypoints, not a straight line.
/// </summary>
public enum TargetPriority
{
    First,
    Last,
    Closest
}
