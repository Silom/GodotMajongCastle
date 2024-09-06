using Godot;

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
        public UnitProvisioner(UnitTypes unitType, bool playerOwned = false, bool showUnitToken = true)
        {
            switch (unitType)
            {
                case UnitTypes.Rat:
                    constructRat(playerOwned, showUnitToken);
                    break;
                case UnitTypes.Pesent:
                    constructPesent(playerOwned, showUnitToken);
                    break;
                case UnitTypes.Knight:
                    constructKnight(playerOwned, showUnitToken);
                    break;
                case UnitTypes.Archer:
                    constructArcher(playerOwned, showUnitToken);
                    break;
                default:
                    break;
            }
        }

        private void constructRat(bool playerOwned, bool showUnitToken)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Rat",
                health = 1,
                attackPower = 1,
                defensePower = 1,
                playerOwned = playerOwned,
                showUnitToken = showUnitToken
            };
        }
        private void constructPesent(bool playerOwned, bool showUnitToken)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Pesent",
                health = 2,
                attackPower = 1,
                defensePower = 1,
                playerOwned = playerOwned,
                showUnitToken = showUnitToken
            };
        }
        private void constructKnight(bool playerOwned, bool showUnitToken)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Knight",
                health = 4,
                attackPower = 2,
                defensePower = 1,
                playerOwned = playerOwned,
                showUnitToken = showUnitToken
            };
        }
        private void constructArcher(bool playerOwned, bool showUnitToken)
        {
            UnitEntity = new Unit()
            {
                enemyName = "Archer",
                health = 1,
                attackPower = 2,
                defensePower = 1,
                playerOwned = playerOwned,
                showUnitToken = showUnitToken
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
        public bool showUnitToken;


        public override void _Ready()
        {
            var label = new Label()
            {
                Text = enemyName,
                Position = Position,
            };
            AddChild(label);
        }

        public override void _Draw()
        {
            GD.PushError(showUnitToken);
            if (showUnitToken)
            {
                DrawCircle(new(0, 0), 32, Colors.Red);
            }
        }
    }
}