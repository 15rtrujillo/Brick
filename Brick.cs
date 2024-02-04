using Godot;
using System;

namespace BrickGame
{
	public partial class Brick : StaticBody2D
	{
		[Export]
		public int PointValue { get; set; }
		[Export]
		public Color BrickColor { get; set; } = Colors.White;
		
		public delegate void BrickHitEventHandler(int points, Vector2 position);
		public BrickHitEventHandler BrickHit;
		
		public override void _Ready()
		{
			GetNode<Sprite2D>("Sprite2D").Modulate = BrickColor;
		}
		
		public void Hit()
		{
			BrickHit(PointValue, GlobalPosition);
			QueueFree();
		}
	}
}
