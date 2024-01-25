using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	public float LaunchSpeed { get; set; } = 300.0f;
	
	private bool _active = false;

	[Signal]
	public delegate void BallDiedEventHandler();
	
	public override void _PhysicsProcess(double delta)
	{
		KinematicCollision2D collision = MoveAndCollide(Velocity * (float)delta);
		
		if (collision != null)
		{
			Vector2 bounceDirection = collision.GetNormal();
			bool hitWall = false;
			
			GD.Print("Name: " + ((Node)collision.GetCollider()).Name);
			GD.Print("Normal: " + collision.GetNormal().ToString());
			if (collision.GetCollider() is StaticBody2D staticBody)
			{
				if (staticBody.IsInGroup("Walls"))
				{
					if (staticBody.Name == "BottomWall")
					{
						EmitSignal(SignalName.BallDied);
						// TODO: QueueFree();
					}

					hitWall = true;
					// bounceDirection = collision.GetNormal();
				}

				else if (staticBody is Brick brick)
				{
					// bounceDirection = brick.GlobalPosition.DirectionTo(Position);
					brick.Hit();
				}
			}

			else if (collision.GetCollider() is CharacterBody2D charBody)
			{
				// bounceDirection = charBody.GlobalPosition.DirectionTo(Position);
			}
			
			float clampingFactor = 1.5f;
			/*
			if (!hitWall)
			{
				bounceDirection = new Vector2(
					x: Mathf.Clamp(bounceDirection.X * clampingFactor, -1.0f, 1.0f),
					y: bounceDirection.Y).Normalized();
			}
			*/
			
			GD.Print("Bounce: " + bounceDirection.ToString());
			Vector2 newVelocity = Velocity.Bounce(bounceDirection);
			if (newVelocity == -Velocity)
			{
				newVelocity = newVelocity.Rotated((float)GD.RandRange(Mathf.Pi / 16.0, Mathf.Pi / 16.0));
			}
			Velocity = newVelocity;
		}
	}
	
	public void Shoot()
	{
		if (_active) return;
		
		Velocity = Vector2.Up * LaunchSpeed;
		_active = true;
	}
}
