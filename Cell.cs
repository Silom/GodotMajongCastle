using Godot;

namespace MajongCastle
{



    public partial class Cell : Area2D
    {
        public Vector2 Size { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private bool Hovered { get; set; } = false;


        private CollisionShape2D collisionShape;

        public override void _Ready()
        {
            SetProcessInput(true);

            collisionShape = new CollisionShape2D();
            AddChild(collisionShape);
            collisionShape.Shape = new RectangleShape2D { Size = Size };

            //collisionShape.Position = Position;

            var shapeRect = collisionShape.Shape.GetRect();
            shapeRect.Position = Position;
            shapeRect.Size = Size;
            //Translate(Position);

            // mhh I need to figure out how to check for event target, otherwise every input event will trigger always
            Connect("mouse_entered", new Callable(this, nameof(OnMouseEntered)));
            Connect("mouse_exited", new Callable(this, nameof(OnMouseExited)));
        }

        private void OnMouseEntered()
        {
            Hovered = true;
            GD.PushError("Hovered: " + GlobalPosition + " - " + Position + " - " + X + ":" + Y);
            QueueRedraw();
        }
        private void OnMouseExited()
        {
            Hovered = false;
            QueueRedraw();
        }

        public override void _Draw()
        {
            // Debug
            //GD.PushError(GlobalPosition + " - " + Position + " - " + X + ":" + Y);
            if (Hovered)
            {
                DrawCircle(new Vector2(0, 0), Size.X / 2, Colors.Green);
            }
        }

        /*public override void _Input(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseButtonEvent)
            {
                if (mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == MouseButton.Left)
                {
                    GD.PushError(GlobalPosition + " - " + Position + " - " + X + ":" + Y);
                    var tree = GetTree();
                    var rootNode = tree.Root.GetNode("Root");
                    GD.PushError(rootNode);
                    //rootNode.Connect("onClick", new Callable(this, "OnClickGridCell"));
                }
            }
        }*/
    }
}