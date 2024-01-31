using Godot;
using System;

namespace BrickGame
{
	public partial class Pickup : RigidBody2D
	{
		[Export]
		public PickupType Type { get; set; } = PickupType.None;
		
		private void OnVisibleOnScreenNotifier2DScreenExited()
		{
			QueueFree();
		}

		private void OnArea2DBodyEntered(Node2D body)
		{
			if (body is Paddle paddle)
			{
				paddle.PickupTouched(Type);
			}
			QueueFree();
		}
	}
}
