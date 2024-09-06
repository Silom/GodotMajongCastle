using Godot;
using System.Collections.Generic;

namespace MajongCastle
{

    public partial class Signals : Node
    {
        [Signal] public delegate void GridCellPressedEventHandler();
    }
}