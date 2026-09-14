using Godot;

namespace DragonXVI.XVIGodot;


/// <summary>
/// Simple interface that mostly just reminds you to disable properties.
/// </summary>
public interface IStrippedNode<T> where T: Node
{
    protected abstract static string[] GetDisabledPropertyNames();
}

