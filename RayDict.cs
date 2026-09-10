namespace DragonXVI.XVIGodot;

/// <summary>
/// Helper class for Godot raycast result dictionaries.
/// </summary>
public static class RayDict
{
    /// <summary>
    /// The colliding object, usually a [Node2D] ([TileMapLayer] or [CollisionObject2D]).
    /// Is null if the object was created from the physics server.
    /// </summary>
    public const string Collider = "collider";
    /// <summary>
    /// The colliding objects id.
    /// Still not sure what this means, go read the docs or smth.
    /// </summary>
    public const string ColliderID = "class_name";
    /// <summary>
    /// Normal vector of the collision.
    /// Can be a zero vector if the collision happens inside an object.
    /// </summary>
    public const string Normal = "normal";
    /// <summary>
    /// Global position of the collision point.
    /// </summary>
    public const string Position = "position";
    /// <summary>
    /// [RID] of the colliding object.
    /// </summary>
    public const string Rid = "rid";
    /// <summary>
    /// Shape index of the hit collider.
    /// </summary>
    public const string Shape = "shape";
}