using Godot;
using System;

public partial class Main : Node
{
	private Paddle _paddle;
	private Ball _ball;
	
	public override void _Ready()
	{
		_paddle = GetNode<Paddle>("Paddle");
		_ball = GetNode<Ball>("Ball");
		
		_paddle.Ball = _ball;
		
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
}
