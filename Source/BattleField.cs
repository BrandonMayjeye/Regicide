using Godot;
using System;
using System.Collections.Generic;

public partial class BattleField : Node2D
{
	public Player Player { get; private set; }
	public Battle Battle { get; private set; }

	private List<Card> _cardsInHand = new List<Card>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Player = this.GetParent() as Player;
		this.Battle = this.Player.GetParent() as Battle;
		this.Player.RegisterBattleField(this);
	}
	public void RegisterCardInHand(Card card)
	{
		this._cardsInHand.Add(card);
		//hook up events as needed

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
