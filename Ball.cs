using Godot;
using System;

public partial class Ball : CharacterBody2D
{
	public float LaunchSpeed { get; set; } = 300.0f;
	
	private bool _active = false;
	
	public override void _PhysicsProcess(double delta)
	{
		var collision = MoveAndCollide(Velocity * (float)delta);
		
		if (collision != null)
		{
			if (collision.GetCollider() is Node collisionNode && collisionNode.IsInGroup("Walls"))
			{
				Velocity = Velocity.Bounce(collision.GetNormal());
			}
			else
			{
				Vector2 direction = (Position - ((Node2D)collision.GetCollider()).Position).Normalized();
				Velocity = Velocity.Bounce(direction);
			}
		}
	}
	
	public void Shoot()
	{
		if (_active) return;
		
		Velocity = Vector2.Up * LaunchSpeed;
		_active = true;
	}
}
