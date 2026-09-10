using Godot;

namespace DragonXVI.XVIGodot;

/// <summary>
/// Utility class that holds helpful godot specific functions.
/// </summary>
public static class XVIFuncs
{
    /// <summary>
    /// Sets all node processes that have a "SetProcess" method.
    /// Please note that setting all of these to true on a node that doesnt use all these functions is a waste, as the engine will call empty methods it would have skipped otherwise.
    /// 
    /// By default this sets all processes to false, especially useful for tool nodes.
    /// </summary>
    /// <param name="node">The node to set processes on.</param>
    /// <param name="enabled">True for enabled, false for disabled. Is false by default.</param>
    public static void SetNodeProcesses( Node node, bool enabled = false )
    {
        node.SetProcess(enabled);
        node.SetPhysicsProcess(enabled);
        node.SetProcessInput(enabled);
        node.SetProcessShortcutInput(enabled);
        node.SetProcessUnhandledInput(enabled);
        node.SetProcessUnhandledKeyInput(enabled);
    }
}