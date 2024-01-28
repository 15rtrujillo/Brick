using Godot;
using System;

public partial class Main : Node
{
	private Paddle _paddle;
	private Ball _ball;
	private UI _ui;
	private int _score;
	
	public override void _Ready()
	{
		_paddle = GetNode<Paddle>("Paddle");
		_ball = GetNode<Ball>("Ball");
		_ui = GetNode<UI>("UI");
		
		_paddle.Ball = _ball;

		ConnectBrickSignals();
		
		// ColorBoard();
	}
	
	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionPressed("fire"))
		{
			_paddle.BallAttached = false;
			_ball.Shoot();
		}
	}

	private void ConnectBrickSignals()
	{
		var children = GetChildren();
		foreach (Node child in children)
		{
			if (child.Name.ToString().Contains("Bricks"))
			{
				var bricks = child.GetChildren();
				foreach (Node subchild in bricks)
				{
					// Ensure we're dealing with a brick
					if (subchild is Brick brick)
					{
						brick.BrickHit += OnBrickHit;
					}
				}
			}
		}
	}
	
	private void ColorBoard()
	{
		Color gameColor = new Color((float)GD.RandRange(0.1f, 1.0f), (float)GD.RandRange(0.1f, 1.0f), (float)GD.RandRange(0.1f, 1.0f));
		
		var children = GetChildren();
		foreach (var child in children)
		{
			Sprite2D sprite = child.GetNodeOrNull<Sprite2D>("Sprite2D");
			if (sprite != null)
			{
				sprite.Modulate = gameColor;
			}
		}
	}

	private void OnBrickHit(int points)
	{
		GameState.Score += points;
		_ui.UpdateScore(GameState.Score);
	}
}
