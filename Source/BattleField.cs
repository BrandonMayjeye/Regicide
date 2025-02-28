using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleField : Node2D
{
	public Player Player { get; private set; }
	public Battle Battle { get; private set; }

	private List<Card> _cardsInHand = new List<Card>();
	private List<CardSlot> _cardSlots = new List<CardSlot>();
	private List<Card> _highlightedCards = new List<Card>();
	// Called when the node enters the scene tree for the first time.
	private Card _draggedCard;
	private Vector2 _draggedCardOriginalPosition;

	public override void _Ready()
	{
		this.Player = this.GetParent() as Player;
		this.Battle = this.Player.GetParent() as Battle;
		this.Player.RegisterBattleField(this);
		this.Battle.OnLeftClick += _onLeftClick;
	}

	private void _onLeftClick(object sender, MouseButtonEventArgs e)
	{
		if (!this.Player.IsMyPlayer || this.Battle.GetCurrentPlayer != Player)
			return;
		GD.Print(e.IsMouseDown);
		var cards = this.Battle.RaycastFor<Card>(1);
		var topCard = cards.FirstOrDefault();
		if (e.IsMouseDown && topCard != null && _highlightedCards.Contains(topCard) && _draggedCard == null)
		{
			_draggedCard = topCard;
			_draggedCardOriginalPosition = _draggedCard.Position;
		}
		else if (!e.IsMouseDown)
		{
			var cardSlots = this.Battle.RaycastFor<CardSlot>(1);
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

	public void RegisterCardInHand(Card card)
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

		var card = sender as Card;
		this._highlightedCards.Add(card);
	}

	private void _cardMouseExited(object sender, EventArgs e)
	{
		if (!this.Player.IsMyPlayer || this.Battle.GetCurrentPlayer != Player)
			return;

		var card = sender as Card;
		this._highlightedCards.Remove(card);
	}
	private Card topCard;
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

	internal void RegisterCardSlot(CardSlot cardSlot)
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
