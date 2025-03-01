using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleFieldNode : Node2D
{
	public PlayerNode Player { get; private set; }
	public BattleNode Battle { get; private set; }

	private List<CardNode> _cardsInHand = new List<CardNode>();
	private List<CardSlotNode> _cardSlots = new List<CardSlotNode>();
	private List<CardNode> _highlightedCards = new List<CardNode>();
	// Called when the node enters the scene tree for the first time.
	private CardNode _draggedCard;
	private Vector2 _draggedCardOriginalPosition;

	public override void _Ready()
	{
		this.Player = this.GetParent() as PlayerNode;
		this.Battle = this.Player.GetParent() as BattleNode;
		this.Player.RegisterBattleField(this);
		this.Battle.OnLeftClick += _onLeftClick;
	}

	private void _onLeftClick(object sender, MouseButtonEventArgs e)
	{
		if (!this.Player.IsMyPlayer || this.Battle.GetCurrentPlayer != Player)
			return;
		GD.Print(e.IsMouseDown);
		var cards = this.Battle.RaycastFor<CardNode>(1);
		var topCard = cards.FirstOrDefault();
		if (e.IsMouseDown && topCard != null && _highlightedCards.Contains(topCard) && _draggedCard == null)
		{
			_draggedCard = topCard;
			_draggedCardOriginalPosition = _draggedCard.Position;
		}
		else if (!e.IsMouseDown)
		{
			var cardSlots = this.Battle.RaycastFor<CardSlotNode>(1);
			var topCardSlot = cardSlots.FirstOrDefault();
			if (topCardSlot != null && topCardSlot.GetCard() == null && this._cardSlots.Contains(topCardSlot) && topCardSlot.IsDelayedSlot)
			{

				topCardSlot.SetCard(this._draggedCard);
			}
			else if (this._draggedCard != null)
			{
				this._draggedCard.Position = _draggedCardOriginalPosition;

			}
			if (this._draggedCard != null)
			{
				this._draggedCard.Scale = this.Battle.CardSize;
				this._draggedCard = null;
			}

		}
	}
	private int inHandNextZIndex => this._cardsInHand.Max(g => g.ZIndex) + 1;

	public void RegisterCardInHand(CardNode card)
	{
		this._cardsInHand.Add(card);

		card.OnCardMouseEntered += _cardMouseEntered;
		card.OnCardMouseExited += _cardMouseExited;
		card.ZIndex = inHandNextZIndex;

		//hook up events as needed

	}

	private void _cardMouseEntered(object sender, EventArgs e)
	{
		if (!this.Player.IsMyPlayer || this.Battle.GetCurrentPlayer != Player)
			return;

		var card = sender as CardNode;
		this._highlightedCards.Add(card);
	}

	private void _cardMouseExited(object sender, EventArgs e)
	{
		if (!this.Player.IsMyPlayer || this.Battle.GetCurrentPlayer != Player)
			return;

		var card = sender as CardNode;
		this._highlightedCards.Remove(card);
	}
	private CardNode topCard;
	private int topCardZIndex;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!this.Player.IsMyPlayer || this.Battle.GetCurrentPlayer != Player)
			return;

		if (_draggedCard != null)
		{
			var mousePos = GetGlobalMousePosition();
			var screenSize = GetViewportRect().Size;
			_draggedCard.Position = new Vector2(Mathf.Clamp(mousePos.X, 0, screenSize.X), Mathf.Clamp(mousePos.Y, 0, screenSize.Y));

		}
		if (this._highlightedCards.Count > 0)
		{
			var cards = this._highlightedCards.OrderByDescending(g => g.ZIndex);
			var cardsEnumerator = cards.GetEnumerator();
			cardsEnumerator.MoveNext();

			if (this.topCard != cardsEnumerator.Current)
			{
				if (this.topCard != null)
				{
					topCard.Scale = Battle.CardSize;
					topCard.ZIndex = this.topCardZIndex;
				}
				this.topCard = cardsEnumerator.Current;
				this.topCardZIndex = this.topCard.ZIndex;
				this.topCard.ZIndex = inHandNextZIndex;
				topCard.Scale = Battle.CardHighlightSize;
				while (cardsEnumerator.MoveNext())
				{
					cardsEnumerator.Current.Scale = Battle.CardSize;

				}
			}

		}
		else if (this.topCard != null)
		{
			topCard.Scale = Battle.CardSize;
			topCard.ZIndex = this.topCardZIndex;

		}
	}

	internal void RegisterCardSlot(CardSlotNode cardSlot)
	{
		this._cardSlots.Add(cardSlot);
	}

	internal void OnNextTurn()
	{
		foreach (var slot in _cardSlots)
		{
			slot.OnNextTurn();
		}
	}
}
