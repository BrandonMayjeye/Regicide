using Godot;
using Mayjeye;
using System;

public partial class GameNode : Node2D
{
	private Game Game;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{


		//create the game instance;
		this.Game = new Game("Development Game", this);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
