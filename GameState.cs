using Godot;
using System;

namespace BrickGame
{
	public static class GameState
	{
		public static int Score { get; set; } = 0;
		public static int HighScore { get; set; } = 0;
		public static int Lives { get; set; } = 5;
		public static int Level { get; set; } = 1;
		
		public static bool IsGameOver()
		{
			return Lives <= 0;
		}
		
		public static void UpdateHighScore()
		{
			// TODO: Load from file and check if bigger than score
			
			HighScore = Score;
			using var saveFile = FileAccess.Open("res://high.score", FileAccess.ModeFlags.Write);
			saveFile.Store32(HighScore);
		}
	}
}
