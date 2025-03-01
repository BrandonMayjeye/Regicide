using Godot;
using System;
namespace Mayjeye.Nodes
{
	public partial class CardSlotNode : Node2D
	{
		[Export]
		public bool IsDelayedSlot = false;
		[Export]
		public int BattleSlotIndex;
		[Export]
		public CardSlotNode DelayedToSlot;
		public BattleFieldNode BattleField { get; private set; }
		public PlayerNode Player { get; private set; }
		public BattleNode Battle { get; private set; }
		public event EventHandler OnCardMouseEntered;
		public event EventHandler OnCardMouseExited;
		private CardNode _card;
		public CardNode GetCard() => _card;


		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			this.BattleField = this.GetParent() as BattleFieldNode;
			this.Player = this.BattleField.GetParent() as PlayerNode;
			this.Battle = this.Player.GetParent() as BattleNode;
			this.BattleField.RegisterCardSlot(this);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}

		internal void SetCard(CardNode draggedCard)
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
}