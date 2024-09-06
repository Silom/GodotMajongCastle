using Godot;
using System.Collections.Generic;

namespace MajongCastle
{

    public class Helpers
    {
        public static void ShiftRight<T>(List<T> lst, int shifts)
        {
            for (int i = lst.Count - shifts - 1; i >= 0; i--)
            {
                lst[i + shifts] = lst[i];
            }

            for (int i = 0; i < shifts; i++)
            {
                lst[i] = default(T);
            }
        }
    }

    public partial class Root : Node2D
    {
        private int cellSize = 64;
        private int gridX = 10;
        private int gridY = 5;
        // TODO this should be organized by some layout component but for the sake of prototyping the grid we remain with this for now
        // 50 is something like a base margin
        private Vector2 marginGameBoardVec = new(50, 150);
        private Vector2 marginIncomingEnemiesVec = new(50, 50);
        private Color gridColor = Colors.White;
        private RandomNumberGenerator rng;

        private int roundTick = 0;

        private Button nextTurnButton;
        private Node2D unitContainer;


        // A list of enemies that move in tound tick
        // TODO this might as well be a actual array for once
        private List<List<Unit>> enemyPipeline = new(); // these enemies will driple down on the playfield whenever one turn passes
        private List<List<Unit>> gameBoard = new(); // x,y placement grid
        private List<Area2D> gridCells = new();

        public override void _Ready()
        {
            rng = new RandomNumberGenerator();
            rng.Randomize();

            nextTurnButton = GetNode<Button>("NextTurn");
            nextTurnButton.Pressed += OnNextTurn;

            unitContainer = GetNode<Node2D>("UnitContainer");

            for (int x = 0; x < gridX; x++)
            {
                enemyPipeline.Add(new List<Unit>());
                gameBoard.Add(new List<Unit>());
                for (int y = 0; y < gridY; y++)
                {
                    // Register signals on click areas for onclick event elegation passing x,y
                    var gridCell = new Cell()
                    {
                        // I dont like that the area2d trys to center based of of its collision shape, I cant seem to find a way to change the anchoring
                        // instead of center we would want top-left in order to draw our grid based on start positions and cell sizes
                        Position = new Vector2(x * cellSize, y * cellSize) + marginGameBoardVec + new Vector2(cellSize, cellSize) / 2,
                        Size = new Vector2(cellSize, cellSize),
                        X = x,
                        Y = y,
                    };
                    unitContainer.AddChild(gridCell);
                    gridCells.Add(gridCell);

                    var amount = rng.RandfRange(1, 3);
                    if (amount <= 2)
                    {
                        var prov = new UnitProvisioner(UnitTypes.Rat, false, true);
                        AddChild(prov.UnitEntity);

                        gameBoard[x].Add(prov.UnitEntity);
                    }
                    else
                    {
                        gameBoard[x].Add(null);
                    }
                }
            }

            QueueRedraw();
        }

        public override void _Draw()
        {
            DrawEnemyPipeline();
            DrawGameGrid();
        }


        // Signal setup for placing player units
        private void OnClickGridCell()
        {
            //var unitToken = new UnitProvisioner(UnitTypes.Pesent, true, true);
            //unitToken.UnitEntity

            // TODO detect which coord was clicked
        }

        private void OnNextTurn()
        {
            roundTick++;

            // very basic loop to move all pieces w/o trait system (ie. move two sqares, go left or right etc.)
            for (int x = gameBoard.Count - 1; x >= 0; x--)
            {
                for (int y = gameBoard[x].Count - 1; y >= 0; y--)
                {
                    var context = gameBoard[x][y];
                    // check if field is filled and if the row is not the last row
                    if (context != null && y < gameBoard[x].Count - 1)
                    {
                        // The one sould be a range value for movement range and it should backwards iterate over the distance that would be possible
                        if (gameBoard[x][y + 1] == null)
                        {
                            gameBoard[x][y + 1] = context;
                            gameBoard[x][y] = null;
                        }
                        // TODO we are going backwards so we can simply check if next slot is free or attack or wait
                    }
                    else if (context != null)
                    {
                        // filled but at the gates => cause dmg to castle
                    }
                }
            }

            // resolve any combat
            // TODO


            // Move incoming enemy pipeline
            // Each tile will allow one enemy to enter the field
            // If an enemy remains it moves to the right
            for (var i = 0; i < enemyPipeline.Count; i++)
            {
                // add one of each stack to the first row
                if (enemyPipeline[i].Count != 0)
                {
                    if (gameBoard[i][0] == null)
                    {
                        gameBoard[i][0] = enemyPipeline[i][0];
                        enemyPipeline[i].RemoveAt(0);
                        gameBoard[i][0].showUnitToken = true;
                        gameBoard[i][0].QueueRedraw();
                    }
                }
            }

            for (var i = enemyPipeline.Count - 2; i >= 0; i--)// skip one index since we go from right
            {
                // shift each index to the right
                enemyPipeline[i + 1] = enemyPipeline[i];
                enemyPipeline[i] = new();
            }

            // generate new enemy set and add to scene
            var newSet = GenerateEnemySet();
            enemyPipeline[0].AddRange(newSet);

            QueueRedraw();
        }

        private void DrawEnemyPipeline()
        {
            // Incoming grid
            for (int x = 0; x <= gridX; x++)
            {
                Vector2 from = new(x * cellSize, 0);
                Vector2 to = new(x * cellSize, 1 * cellSize);
                DrawLine(from + marginIncomingEnemiesVec, to + marginIncomingEnemiesVec, Colors.Pink);
            }
            for (int y = 0; y <= 1; y++)
            {
                Vector2 from = new(0, y * cellSize);
                Vector2 to = new(gridX * cellSize, y * cellSize);
                DrawLine(from + marginIncomingEnemiesVec, to + marginIncomingEnemiesVec, Colors.Pink);
            }


            // Position everything
            for (var x = 0; x < enemyPipeline.Count; x++)
            {
                var pos = new Vector2(x * cellSize, 0) + marginIncomingEnemiesVec;// spacervec
                for (var y = 0; y < enemyPipeline[x].Count; y++)
                {
                    // Position all elements inside the line grid
                    if (enemyPipeline[x].Count != 0)
                    {
                        enemyPipeline[x][y].Position = pos + new Vector2(0, 10 * y);
                    }
                }
            }
        }

        private void DrawGameGrid()
        {
            // Draw the board lines
            for (int x = 0; x <= gridX; x++)
            {
                Vector2 from = new(x * cellSize, 0);
                Vector2 to = new(x * cellSize, gridY * cellSize);
                DrawLine(from + marginGameBoardVec, to + marginGameBoardVec, gridColor);
            }

            for (int y = 0; y <= gridY; y++)
            {
                Vector2 from = new(0, y * cellSize);
                Vector2 to = new(gridX * cellSize, y * cellSize);
                DrawLine(from + marginGameBoardVec, to + marginGameBoardVec, gridColor);
            }

            // Update all positions for enemies on the grid
            var spacervec = new Vector2(-32, -32);
            for (var x = 0; x < gameBoard.Count; x++)
            {
                for (var y = 0; y < gameBoard[x].Count; y++)
                {
                    var context = gameBoard[x][y];

                    var pos = new Vector2(x * cellSize, y * cellSize) + marginGameBoardVec - spacervec;

                    if (context != null)
                    {
                        context.Position = pos;
                    }
                }
            }
        }

        // Generate a set of enemies and put them into the scene, usally this will be pushed onto the incoming enemy pipeline
        private List<Unit> GenerateEnemySet()
        {
            var amount = rng.RandfRange(1, gridX - 5);// TEST REMOVE
            var newEnemies = new List<Unit>();
            for (int i = 0; i < amount; i++)
            {
                // TODO implement a seed and powerlevel ddynamic for randomized results
                var provisionedEnemy = new UnitProvisioner(UnitTypes.Rat, false, false);
                newEnemies.Add(provisionedEnemy.UnitEntity);
                AddChild(provisionedEnemy.UnitEntity);
            }
            return newEnemies;
        }
    }
}