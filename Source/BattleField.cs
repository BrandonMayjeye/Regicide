using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BattleField : Node2D
{
	public Player Player { get; private set; }
	public Battle Battle { get; private set; }

	private List<Card> _cardsInHand = new List<Card>();
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
			this._draggedCard.Position = _draggedCardOriginalPosition;
			this._draggedCard = null;
		}
	}

	public void RegisterCardInHand(Card card)
	{
		this._cardsInHand.Add(card);
		card.OnCardMouseEntered += _cardMouseEntered;
		card.OnCardMouseExited += _cardMouseExited;


		//hook up events as needed

	}

	private void _cardMouseEntered(object sender, EventArgs e)
	{
		var card = sender as Card;
		this._highlightedCards.Add(card);

		card.Scale = Battle.CardHighlightSize;
	}

	private void _cardMouseExited(object sender, EventArgs e)
	{
		var card = sender as Card;
		this._highlightedCards.Remove(card);

		card.Scale = Battle.CardSize;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_draggedCard != null)
		{
			var mousePos = GetGlobalMousePosition();
			var screenSize = GetViewportRect().Size;
			_draggedCard.Position = new Vector2(Mathf.Clamp(mousePos.X, 0, screenSize.X), Mathf.Clamp(mousePos.Y, 0, screenSize.Y));

		}
	}

}
