using Godot;
using System;

public partial class CardNode : Node2D
{
	public BattleFieldNode BattleField { get; private set; }
	public PlayerNode Player { get; private set; }
	public BattleNode Battle { get; private set; }
	public event EventHandler OnCardMouseEntered;
	public event EventHandler OnCardMouseExited;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BattleField = this.GetParent() as BattleFieldNode;
		this.Player = this.BattleField.GetParent() as PlayerNode;
		this.Battle = this.Player.GetParent() as BattleNode;
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
