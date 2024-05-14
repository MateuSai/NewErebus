#if TOOLS
using Godot;
using System;

namespace ErebusLogger;

[Tool]
public partial class ErebusLogger : EditorPlugin
{
    public override void _EnterTree()
    {
        // Initialization of the plugin goes here.
    }

    public override void _ExitTree()
    {
        // Clean-up of the plugin goes here.
    }
}
#endif
