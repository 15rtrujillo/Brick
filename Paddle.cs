using Godot;
using System;
using System.Threading.Tasks;

namespace BrickGame
{
	public partial class Paddle : CharacterBody2D
	{
		public float Speed { get; set; } = 300.0f;
		public Ball AttachedBall { get; set; }
		public bool BallAttached { get; set; } = true;
		
		public delegate void ApplyPickupEventHandler(PickupType pickupType);
		public ApplyPickupEventHandler ApplyPickup;
		
		private bool _bigPaddle = false;

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
		
		public async void BigPaddle()
		{
			CollisionShape2D shape = GetNode<CollisionShape2D>("CollisionShape2D");
			RectangleShape2D rectangle = (RectangleShape2D)shape.Shape;
			Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
			
			Vector2 shapeSize = rectangle.Size;
			Vector2 spriteScale = sprite.Scale;
			_bigPaddle = true;
			rectangle.Size = shapeSize * new Vector2(2, 1);
			sprite.Scale = spriteScale * new Vector2(2, 1);
			
			await Task.Delay(10000);
			
			rectangle.Size = shapeSize;
			sprite.Scale = spriteScale;
			_bigPaddle = false;
		}
		
		public void PickupTouched(PickupType pickupType)
		{
			if (pickupType == PickupType.OneUp)
			{
				// ApplyPickup(pickupType);
			}
			
			else if (pickupType == PickupType.BigPaddle
			&& !_bigPaddle)
			{
				BigPaddle();
			}
		}
	}
}
