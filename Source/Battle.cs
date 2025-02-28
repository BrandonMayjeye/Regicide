using Godot;
using System;

public partial class Battle : Node2D
{
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
