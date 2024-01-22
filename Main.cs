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
	}
	
	public override void _UnhandledKeyInput(InputEvent @event)
	{
		if (@event.IsActionPressed("fire"))
		{
			_paddle.BallAttached = false;
			_ball.Shoot();
		}
	}
}
