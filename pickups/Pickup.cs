using Godot;
using System;

namespace BrickGame
{
	public partial class Pickup : RigidBody2D
	{
		private void OnVisibleOnScreenNotifier2DScreenExited()
		{
			QueueFree();
		}

		private void OnArea2DBodyEntered(Node2D body)
		{
			GD.Print("Hit!");
		}

    }
}
