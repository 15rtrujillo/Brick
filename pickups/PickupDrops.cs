using Godot;
using System;

namespace BrickGame
{
	public class PickupDrops
	{
		private System.Collections.Generic.List<Drop> _drops = new System.Collections.Generic.List<Drop>();
		
		public PickupDrops()
		{
			_drops.Add(new Drop(ResourceLoader.Load<PackedScene>("res://pickups/OneUp.tscn"), 50));
			_drops.Add(new Drop(ResourceLoader.Load<PackedScene>("res://pickups/BigPaddle.tscn"), 45));
			_drops.Add(new Drop(5));
		}
		
		public PackedScene RollPickup()
		{
			int roll = GD.RandRange(1, GetTotalWeight());
			int current = 0;
			for (int i = 0; i < _drops.Count; ++i)
			{
				current += _drops[i].Weight;
				if (roll <= current)
				{
					return _drops[i].PickupScene;
				}
			}
			
			return null;
		}
		
		private int GetTotalWeight()
		{
			int totalWeight = 0;
			foreach (Drop drop in _drops)
			{
				totalWeight += drop.Weight;
			}
			return totalWeight;
		}
	}
}
