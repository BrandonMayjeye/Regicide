using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Battle : Node2D
{
	public Vector2 CardSize = new Vector2(0.25f, 0.25f);
	public Vector2 CardHighlightSize = new Vector2(0.35f, 0.35f);
	public event EventHandler<MouseButtonEventArgs> OnLeftClick;

	public override void _Input(InputEvent iEvent)
	{
		if (iEvent is InputEventMouseButton)
		{
			var mouseButtonEvent = iEvent as InputEventMouseButton;
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left)
			{
				this._leftClick(mouseButtonEvent.Pressed);
			}
		}
	}

	private void _leftClick(bool mouseDown)
	{
		if (OnLeftClick != null)
		{
			OnLeftClick.Invoke(this, new MouseButtonEventArgs(mouseDown));
		}
	}


	public IEnumerable<T> RaycastFor<T>(uint collisionMask) where T : Node2D
	{
		var spacestate = GetWorld2D().DirectSpaceState;
		var pars = new PhysicsPointQueryParameters2D();
		pars.Position = GetGlobalMousePosition();
		pars.CollideWithAreas = true;
		pars.CollisionMask = collisionMask;
		var result = spacestate.IntersectPoint(pars);
		List<T> raycastResults = new List<T>();
		if (result.Count > 0)
		{
			foreach (var cast in result)
			{
				GD.Print("Raycast results greater than 0", cast);

				var castCollider = (cast["collider"].Obj) as Area2D;
				var parentNode = castCollider.GetParent() as Node2D;
				if (typeof(T).IsAssignableFrom(parentNode.GetType()))
				{
					raycastResults.Add((T)parentNode);
				}
			}
		}
		return raycastResults.OrderBy(g => g.ZIndex);
	}

	public Player Player;
	// Called when the node enters the scene tree for the first time.
	public void RegisterPlayer(Player player) => this.Player = player;
	public override void _Ready()
	{
		GD.Print("Battle Ready");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


}

public class MouseButtonEventArgs
{
	public bool IsMouseDown;

	public MouseButtonEventArgs(bool isMouseDown)
	{
		this.IsMouseDown = isMouseDown;
	}
}
/*

if (@event is InputEventMouseButton)
{
   var mouseEvent = @event as InputEventMouseButton;
   if (mouseEvent.ButtonIndex == MouseButton.Left)
   {
       if (mouseEvent.Pressed)
       {
           GD.Print("Left Pressed");
           if (TryRaycastForCard(out var variant))
           {
               DragCard(variant.GetParent() as Card);

           }
           else
           {
               if (CardBeingDragged != null)
               {
                   DropCard();

               }
           }

       }
       else
       {
           if (CardBeingDragged != null)
           {
               DropCard();

           }


       }


   }
}
base._Input(@event);
*/