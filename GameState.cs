using Godot;
using System;

namespace BrickGame
{
	public static class GameState
	{
		public static int Score { get; set; } = 0;
		public static int HighScore { get; private set; } = 0;
		public static int Lives { get; set; } = 4;
		public static int Level { get; set; } = 1;

		private static readonly string _highScoreFilePath = "res://high.score";
		
		public static bool IsGameOver()
		{
			return Lives < 0;
		}

		public static void LoadHighScore()
		{
            if (FileAccess.FileExists(_highScoreFilePath))
            {
                using FileAccess highScoreFile = FileAccess.Open(_highScoreFilePath, FileAccess.ModeFlags.Read);
                HighScore = (int)highScoreFile.Get32();
            }
        }
		
		public static void UpdateHighScore()
		{
			if (Score <= HighScore) return;

            HighScore = Score;

			using FileAccess highScoreFile = FileAccess.Open(_highScoreFilePath, FileAccess.ModeFlags.Write);
			highScoreFile.Store32((uint)HighScore);
            
		}
	}
}
