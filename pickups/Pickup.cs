using Godot;
using System;

namespace BrickGame
{
	public partial class Pickup : Area2D
	{
		public Vector2 Velocity { get; set; } = Vector2.Zero;
		
		private float _gravity;
		
		public override void _Ready()
		{
			_gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");
		}
		
		public override void _PhysicsProcess(double delta)
		{
			// FIXME: Make the powerup go up and back down
			// Position += Velocity;
		}
		
		private void OnVisibleOnScreenNotifier2DScreenExited()
		{
			QueueFree();
		}
	}
}
