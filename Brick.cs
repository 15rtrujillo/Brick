using Godot;
using System;

public partial class Brick : StaticBody2D
{
	[Export]
	public int PointValue { get; set; }
	[Export]
	public Color BrickColor { get; set; } = Colors.White;
	
	[Signal]
	public delegate void BrickHitEventHandler(int points);
	
	public override void _Ready()
	{
		GetNode<Sprite2D>("Sprite2D").Modulate = BrickColor;
	}
	
	public void Hit()
	{
		EmitSignal(SignalName.BrickHit, PointValue);
		QueueFree();
	}
}
