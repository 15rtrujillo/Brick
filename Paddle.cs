using Godot;
using System;
using System.Threading.Tasks;

namespace BrickGame
{
	public partial class Paddle : CharacterBody2D
	{
		public float Speed { get; set; } = 300.0f;
		
		public delegate void ApplyPickupEventHandler(PickupType pickupType);
		public ApplyPickupEventHandler ApplyPickup;
		
		private Ball _attachedBall;
		private bool _ballAttached;
		private bool _bigPaddle = false;
		private bool _magnet = false;

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
			
			Vector2 oldPaddlePos = Position;
			
			Velocity *= Speed;
			MoveAndCollide(Velocity * (float)delta);
			
			if (_ballAttached)
			{
				_attachedBall.Position += Position - oldPaddlePos;
			}
		}
		
		public override void _UnhandledKeyInput(InputEvent @event)
		{
			if (@event.IsActionPressed("fire"))
			{
				if (!GameState.Running) return;
				
				_ballAttached = false;
				_attachedBall.Shoot();
			}
		}
		
		public void AttachNewBall(Ball ball)
		{
			_attachedBall = ball;
			_ballAttached = true;
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
		
		public void BallTouched()
		{
			if (!_magnet) return;
			_ballAttached = true;
		}
		
		public void PickupTouched(PickupType pickupType)
		{
			if (pickupType == PickupType.BigPaddle
			&& !_bigPaddle)
			{
				BigPaddle();
				return;
			}
			
			ApplyPickup(pickupType);
		}
	}
}
