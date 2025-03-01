using Godot;
using System;

public partial class Card : Node2D
{
	public BattleField BattleField { get; private set; }
	public Player Player { get; private set; }
	public Battle Battle { get; private set; }
	public event EventHandler OnCardMouseEntered;
	public event EventHandler OnCardMouseExited;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BattleField = this.GetParent() as BattleField;
		this.Player = this.BattleField.GetParent() as Player;
		this.Battle = this.Player.GetParent() as Battle;
		this.BattleField.RegisterCardInHand(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void onMouseEntered()
	{
		if (OnCardMouseEntered != null)
		{
			OnCardMouseEntered.Invoke(this, new EventArgs());
		}
	}
	private void onMouseExited()
	{
		if (OnCardMouseExited != null)
		{
			OnCardMouseExited.Invoke(this, new EventArgs());
		}

	}
}
