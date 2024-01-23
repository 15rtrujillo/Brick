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
			Vector2 bounceDirection = Vector2.Zero;

			if (collision.GetCollider() is StaticBody2D staticBody)
			{
				if (staticBody.IsInGroup("Walls"))
				{
					if (staticBody.Name == "BottomWall")
					{
						EmitSignal(SignalName.BallDied);
						// TODO: QueueFree();
					}

					bounceDirection = collision.GetNormal();
                }

				else if (staticBody is Brick brick)
				{
					brick.Hit();
					bounceDirection = (Position - brick.Position).Normalized();
				}
			}

			else if (collision.GetCollider() is CharacterBody2D charBody)
			{
				bounceDirection = (Position - charBody.Position).Normalized();
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
