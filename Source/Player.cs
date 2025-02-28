using Godot;
using System;

public partial class Player : Node2D
{

	public Battle Battle { get; private set; }
	public BattleField BattleField { get; private set; }
	public void RegisterBattleField(BattleField battleField) => this.BattleField = battleField;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Battle = this.GetParent() as Battle;// set  up dependencies

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
