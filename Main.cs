using Godot;
using System;
using System.Threading.Tasks;

using BrickGame.Pickups;

namespace BrickGame
{
	public partial class Main : Node
	{	
		private Paddle _paddle;
		private Ball _ball;
		private UI _ui;
		private int _brickCount = 0;
		private PickupDrops _pickupDrops = new PickupDrops();
		
		public override void _Ready()
		{
			_paddle = GetNode<Paddle>("Paddle");
			_ball = GetNode<Ball>("Ball");
			_ui = GetNode<UI>("UI");
			
			_paddle.AttachNewBall(_ball);
			
			_paddle.Speed += GetLevelSpeedModifier() * 0.3f;
			_ball.LaunchSpeed += GetLevelSpeedModifier();
			
			PhysicsServer2D.AreaSetParam(
				GetViewport().FindWorld2D().Space,
				PhysicsServer2D.AreaParameter.Gravity,
				300 + GetLevelSpeedModifier());

			_paddle.ApplyPickup = OnApplyPickup;
			_ball.BallDied = OnBallDied;
			_ui.StartGame = OnStartGame;

			GameState.LoadHighScore();
			UpdateUI();
			ConnectBrickSignals();
			// ColorBoard();
		}
		
		public override void _UnhandledKeyInput(InputEvent @event)
		{
			// CHEATS!?!?!
			if (@event is InputEventKey keyEvent)
			{
				if (keyEvent.IsPressed() && keyEvent.Keycode == Key.N)
				{
					Win();
				}
			}
		}
		
		private void UpdateUI()
		{
			_ui.UpdateHighScore(GameState.HighScore);
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
		
		private float GetLevelSpeedModifier()
		{
			return (GameState.Level - 1) * 75.0f;
		}
		
		private async void GameOver()
		{
			GameState.UpdateHighScore();
			GameState.Lives = 5;
			
			_ui.UpdateHighScore(GameState.HighScore);
			_ui.GameOver();
			await Task.Delay(3000);
			GetTree().ReloadCurrentScene();
		}
		
		private async void Win()
		{
			_ball.QueueFree();

			GameState.UpdateHighScore();
			GameState.Level += 1;

			_ui.UpdateHighScore(GameState.HighScore);
			_ui.Win();
			await Task.Delay(3000);
			GetTree().ReloadCurrentScene();
		}

		private void OnBrickHit(int points, Vector2 position)
		{
			GameState.Score += points;
			_ui.UpdateScore(GameState.Score);
			
			_brickCount--;

			if (_brickCount <= 0)
			{
				Win();
				return;
			}
			
			PackedScene pickup = _pickupDrops.RollPickup();
			if (pickup == null) return;
			Pickup pickupNode = pickup.Instantiate<Pickup>();
			AddChild(pickupNode);
			pickupNode.Position = position;
			
			Vector2 direction = Vector2.Up.Rotated((float)GD.RandRange(-Mathf.Pi / 16, Mathf.Pi / 16)).Normalized();
			pickupNode.ApplyImpulse(direction * (250.0f + GetLevelSpeedModifier() * 0.5f));
			pickupNode.ApplyTorqueImpulse(direction.X * 1800.0f);
		}
		
		private void OnApplyPickup(PickupType type)
		{
			if (type == PickupType.OneUp)
			{
				GameState.Lives += 1;
			}
		}
		
		private void OnBallDied()
		{
			GameState.Lives -= 1;
			_ui.UpdateLives(GameState.Lives);
			
			if (GameState.IsGameOver())
			{
				GameOver();
				return;
			}
			
			_ball = ResourceLoader.Load<PackedScene>("res://Ball.tscn").Instantiate<Ball>();
			AddChild(_ball);
			_ball.Position = _paddle.Position + (Vector2.Up * 25.0f);
			_ball.LaunchSpeed += GetLevelSpeedModifier();
			_ball.BallDied = OnBallDied;
			_paddle.AttachNewBall(_ball);
		}
		
		private void OnStartGame()
		{
			GameState.Running = true;
		}
	}
}
