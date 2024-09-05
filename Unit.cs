using Godot;
using System.Collections.Generic;

namespace MajongCastle
{
    public enum UnitTypes
    {
        Rat,
        Pesent,
        Knight,
        Archer
    }

    // temporary class for retriving some different type of unit used by the player as well the game
    public class UnitProvisioner
    {
        public Unit UnitEntity { get; set; }
        public UnitProvisioner(UnitTypes unitType, bool playerOwned = false)
        {
            switch (unitType)
            {
                case UnitTypes.Rat:
                    constructRat(playerOwned);
                    break;
                case UnitTypes.Pesent:
                    constructRat(playerOwned);
                    break;
                case UnitTypes.Knight:
                    constructRat(playerOwned);
                    break;
                case UnitTypes.Archer:
                    constructRat(playerOwned);
                    break;
                default:
                    break;
            }
        }

        private void constructRat(bool playerOwned)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Rat",
                health = 1,
                attackPower = 1,
                defensePower = 1,
                playerOwned = playerOwned
            };
        }
        private void constructPesent(bool playerOwned)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Pesent",
                health = 2,
                attackPower = 1,
                defensePower = 1,
                playerOwned = playerOwned
            };
        }
        private void constructKnight(bool playerOwned)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Knight",
                health = 4,
                attackPower = 2,
                defensePower = 1,
                playerOwned = playerOwned
            };
        }
        private void constructArcher(bool playerOwned)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Archer",
                health = 1,
                attackPower = 2,
                defensePower = 1,
                playerOwned = playerOwned
            };
        }
    }

    public partial class Unit : Node2D
    {
        public string enemyName;
        public int health;
        public int attackPower;
        public int defensePower;
        public bool playerOwned;

        private List<Trait> traits;

        public override void _Ready()
        {
            RenderNode();
            QueueRedraw();
        }

        public override void _Process(double delta)
        {
        }

        public override void _Draw()
        {
        }

        public void RenderNode()
        {
            DrawCircle(Position, 32, Colors.Red);
            var label = new Label()
            {
                Text = enemyName,
                Position = Position,
            };
            AddChild(label);
        }
    }
}