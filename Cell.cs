using Godot;

namespace MajongCastle
{



    public partial class Cell : Area2D
    {
        public Vector2 Size { get; set; }
        public int X { get; set; }
        public int Y { get; set; }


        private CollisionShape2D collisionShape;

        public override void _Ready()
        {
            collisionShape = new CollisionShape2D();
            AddChild(collisionShape);

            collisionShape.Shape = new RectangleShape2D { Size = Size };
        }
    }
}