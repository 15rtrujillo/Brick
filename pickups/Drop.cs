using Godot;
using System;

namespace BrickGame.Pickups
{
	public class Drop
	{
		public PackedScene PickupScene { get; set; }
		public int Weight { get; set; }
		
		public Drop(PackedScene pickup, int weight)
		{
			PickupScene = pickup;
			Weight = weight;
		}
		
		public Drop(int weight)
		{
			PickupScene = null;
			Weight = weight;
		}
	}
}
