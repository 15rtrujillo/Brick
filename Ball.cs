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
			
			if (collision.GetCollider() is StaticBody2D staticBody)
			{
				if (staticBody.IsInGroup("Walls"))
				{
					if (staticBody.Name == "BottomWall")
					{
						EmitSignal(SignalName.BallDied);
						// TODO: QueueFree(); return;
					}
				}

				else if (staticBody is Brick brick)
				{
					brick.Hit();
				}
			}

			else if (collision.GetCollider() is CharacterBody2D charBody)
			{
				float relativeCollisionPosition = (collision.GetPosition().X - charBody.GlobalPosition.X) / (charBody.GetNode<Sprite2D>("Sprite2D").Scale.X / 2.0f);
				GD.Print("Relative Collision Position: " + relativeCollisionPosition.ToString());
				bounceDirection = new Vector2(relativeCollisionPosition, -1).Normalized();
			}
			
			float dotProduct = Velocity.Normalized().Dot(bounceDirection.Normalized());

			// Protect against shallow bounces
			float threshold = -0.97f;

			if (dotProduct < threshold)
			{
				 GD.Print("Too shallow!");
			}
			
			// Protect against the case where the vectors are nearly perpendicular
			threshold = 0.5f;
			
			if (dotProduct < (0 + threshold) && dotProduct > (0 - threshold))
			{
				GD.Print("Saved a bad bounce");
				bounceDirection = collision.GetNormal();
			}
			
			Velocity = Velocity.Bounce(bounceDirection);
		}
	}
	
	public void Shoot()
	{
		if (_active) return;
		
		Velocity = Vector2.Up * LaunchSpeed;
		_active = true;
	}
}
