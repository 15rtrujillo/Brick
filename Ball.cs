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
				//bounceDirection = charBody.GlobalPosition.DirectionTo(Position);
				float relativeCollisionPosition = (collision.GetPosition().X - charBody.GlobalPosition.X) / (charBody.GetNode<Sprite2D>("Sprite2D").Scale.X / 2.0f);
				GD.Print("Global Paddle Position: " + charBody.Position.ToString());
				GD.Print("Global Collision Position: " + collision.GetPosition().ToString());
				GD.Print("Relative Collision Point: " + relativeCollisionPosition.ToString());
				bounceDirection = new Vector2(relativeCollisionPosition, -1).Normalized();
			}
			
			/*
			float clampingFactor = 1.5f;
			if (!hitWall)
			{
				bounceDirection = new Vector2(
					x: Mathf.Clamp(bounceDirection.X * clampingFactor, -1.0f, 1.0f),
					y: bounceDirection.Y).Normalized();
			}
			*/
			
			GD.Print("Incoming Direction: " + bounceDirection.ToString());
			
			Vector2 newVelocity = Velocity.Bounce(bounceDirection);
			
			GD.Print("Bounce Direction: " + newVelocity.Normalized().ToString());
			
			// Protect against shallow bounces
			float dotProduct = Velocity.Normalized().Dot(newVelocity.Normalized());
			
			float threshold = -0.97f;
			GD.Print("Dot Product: " + dotProduct.ToString());
			if (dotProduct < threshold)
			{
				GD.Print("Too shallow!");
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
