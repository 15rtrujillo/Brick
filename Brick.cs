using Godot;
using System;

public partial class Brick : StaticBody2D
{
	[Export]
	public int PointValue { get; set; }

	[Signal]
	public delegate void BrickHitEventHandler(int points);

	public void Hit()
	{
		EmitSignal(SignalName.BrickHit, PointValue);
		QueueFree();
	}
}
