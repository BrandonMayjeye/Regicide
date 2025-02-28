using Godot;
using System;

public partial class CardSlot : Node2D
{
	[Export]
	public bool IsDelayedSlot = false;
	[Export]
	public int BattleSlotIndex;
	[Export]
	public CardSlot DelayedToSlot;
	public BattleField BattleField { get; private set; }
	public Player Player { get; private set; }
	public Battle Battle { get; private set; }
	public event EventHandler OnCardMouseEntered;
	public event EventHandler OnCardMouseExited;
	private Card _card;
	public Card GetCard() => _card;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BattleField = this.GetParent() as BattleField;
		this.Player = this.BattleField.GetParent() as Player;
		this.Battle = this.Player.GetParent() as Battle;
		this.BattleField.RegisterCardSlot(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	internal void SetCard(Card draggedCard)
	{
		this._card = draggedCard;
		this._card.Position = this.Position;
	}

	internal void OnNextTurn()
	{
		if (this.IsDelayedSlot && this.DelayedToSlot.GetCard() == null && this._card != null)
		{
			this.DelayedToSlot.SetCard(this._card);
			this._card = null;
		}
	}
}
