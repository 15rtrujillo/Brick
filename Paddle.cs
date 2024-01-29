using Godot;
using System;

public partial class Paddle : CharacterBody2D
{
	public float Speed { get; set; } = 300.0f;
	
	public CharacterBody2D AttachedBall { get; set; }
	public bool BallAttached { get; set; } = true;

	public override void _PhysicsProcess(double delta)
	{
		Velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_left"))
		{
			Velocity += Vector2.Left;
		}
		
		if (Input.IsActionPressed("move_right"))
		{
			Velocity += Vector2.Right;
		}
		
		Velocity *= Speed;
		MoveAndCollide(Velocity * (float)delta);
		
		if (BallAttached)
		{
			AttachedBall.Position = Position + (Vector2.Up * 25.0f);
		}
	}
}
