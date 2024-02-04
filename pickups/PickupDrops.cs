using Godot;
using System;

namespace BrickGame.Pickups
{
	public class PickupDrops
	{
		private System.Collections.Generic.List<Drop> _drops = new System.Collections.Generic.List<Drop>();
		
		public PickupDrops()
		{
			_drops.Add(new Drop(ResourceLoader.Load<PackedScene>("res://pickups/OneUp.tscn"), 1));
			_drops.Add(new Drop(ResourceLoader.Load<PackedScene>("res://pickups/BigPaddle.tscn"), 10));
			_drops.Add(new Drop(ResourceLoader.Load<PackedScene>("res://pickups/Magnet.tscn"), 5));
			_drops.Add(new Drop(84));
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
