using System.Collections.Generic;

namespace MajongCastle
{

    public struct Trait
    {
        string name;
        string description;
        string trigger;
    }

    public partial class Enemy : Unit
    {
        private List<Trait> traits;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}