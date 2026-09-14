using Godot;

namespace DragonXVI.XVIGodot;


/// <summary>
/// Interface that adds a node refrence static property to classes used for autoloads.
/// This static property should be set in enter tree, or at a similar time.
/// </summary>
public interface IAutoload<T> where T: Node
{
    public abstract static T Instance { get; set; }
}
