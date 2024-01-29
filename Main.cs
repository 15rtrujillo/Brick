using Godot;
using System;

namespace BrickGame
{
	public partial class Main : Node
	{
		private Paddle _paddle;
		private Ball _ball;
		private UI _ui;
		private int _brickCount = 0;
		private bool _started = false;
		
		public override void _Ready()
		{
			_paddle = GetNode<Paddle>("Paddle");
			_ball = GetNode<Ball>("Ball");
			_ui = GetNode<UI>("UI");
			
			_paddle.AttachedBall = _ball;

			_ball.BallDied = OnBallDied;
			_ui.StartGame = OnStartGame;
			
			UpdateUI();
			ConnectBrickSignals();
			// ColorBoard();
		}
		
		public override void _UnhandledKeyInput(InputEvent @event)
		{
			if (@event.IsActionPressed("fire"))
			{
				if (!_started) return;
				
				_paddle.BallAttached = false;
				_ball.Shoot();
			}
		}
		
		private void UpdateUI()
		{
			_ui.UpdateScore(GameState.Score);
			_ui.UpdateLives(GameState.Lives);
			_ui.UpdateMessage($"Brick - Level {GameState.Level}");
		}

		private void ConnectBrickSignals()
		{
			var children = GetChildren();
			foreach (Node child in children)
			{
				if (child.Name.ToString().Contains("Bricks"))
				{
					var bricks = child.GetChildren();
					foreach (Node subchild in bricks)
					{
						// Ensure we're dealing with a brick
						if (subchild is Brick brick)
						{
							brick.BrickHit = OnBrickHit;
							_brickCount++;
						}
					}
				}
			}
		}
		
		private void ColorBoard()
		{
			Color gameColor = new Color((float)GD.RandRange(0.1f, 1.0f), (float)GD.RandRange(0.1f, 1.0f), (float)GD.RandRange(0.1f, 1.0f));
			
			var children = GetChildren();
			foreach (var child in children)
			{
				Sprite2D sprite = child.GetNodeOrNull<Sprite2D>("Sprite2D");
				if (sprite != null)
				{
					sprite.Modulate = gameColor;
				}
			}
		}
		
		async private void GameOver()
		{
			GameState.UpdateHighScore();
			GameState.Lives = 5;
			
			_ui.UpdateHighScore(GameState.HighScore);
			_ui.GameOver();
			await ToSignal(GetTree().CreateTimer(3.0), SceneTreeTimer.SignalName.Timeout);
			GetTree().ReloadCurrentScene();
		}
		
		private void Win()
		{
			GameState.UpdateHighScore();
		}

		private void OnBrickHit(int points)
		{
			GameState.Score += points;
			_ui.UpdateScore(GameState.Score);
			
			_brickCount--;
		}
		
		private void OnBallDied()
		{
			GameState.Lives -= 1;
			_ui.UpdateLives(GameState.Lives);
			
			if (GameState.IsGameOver())
			{
				GameOver();
			}
			
			_ball = ResourceLoader.Load<PackedScene>("res://Ball.tscn").Instantiate<Ball>();
			AddChild(_ball);
			_ball.BallDied = OnBallDied;
			_paddle.AttachedBall = _ball;
			_paddle.BallAttached = true;
		}
		
		private void OnStartGame()
		{
			_started = true;
		}
	}
}
