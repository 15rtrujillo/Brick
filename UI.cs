using Godot;
using System;

namespace BrickGame
{
	public partial class UI : Control
	{
		private Label _scoreLabel;
		private Label _livesLabel;
		private ColorRect _startScreen;
		private Label _messageLabel;
		private Button _startButton;
		private Label _highScoreLabel;
		
		public delegate void StartGameEventHandler();
		public StartGameEventHandler StartGame;

		public override void _Ready()
		{
			_scoreLabel = GetNode<Label>("ScoreLabel");
			_livesLabel = GetNode<Label>("LivesLabel");
			_startScreen = GetNode<ColorRect>("StartScreen");
			_messageLabel = GetNode<Label>("StartScreen/MessageLabel");
			_startButton = GetNode<Button>("StartScreen/StartButton");
			_highScoreLabel = GetNode<Label>("StartScreen/HighScoreLabel");
		}

		public void UpdateScore(int newScore)
		{
			_scoreLabel.Text = "Score: " + newScore.ToString();
		}

		public void UpdateLives(int newLives)
		{
			_livesLabel.Text = "Lives: " + newLives.ToString();
		}
		
		public void UpdateMessage(string message)
		{
			_messageLabel.Text = message;
		}
		
		public void UpdateHighScore(int newHighScore)
		{
			_highScoreLabel.Text = "High Score: " + newHighScore.ToString();
		}
		
		public void GameOver()
		{
			UpdateMessage("Game Over!");
			_startScreen.Show();
			_startButton.Hide();
		}
		
		private void OnStartButtonPressed()
		{
			_startScreen.Hide();
			StartGame();
		}
	}
}
