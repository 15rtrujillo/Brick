using Godot;
using System;
using System.Threading.Tasks;

namespace BrickGame
{
	public partial class Main : Node
	{	
		private Paddle _paddle;
		private Ball _ball;
		private UI _ui;
		private int _brickCount = 0;
		private bool _started = false;
		private PickupDrops _pickupDrops = new PickupDrops();
		
		public override void _Ready()
		{
			_paddle = GetNode<Paddle>("Paddle");
			_ball = GetNode<Ball>("Ball");
			_ui = GetNode<UI>("UI");
			
			_paddle.AttachedBall = _ball;

			_ball.LaunchSpeed += GetLevelSpeedModifier();
			
			/* Research
			PhysicsServer2D.AreaSetParam(
				GetWorld2D().GetSpace(),
				2,
				new Vector2(0,0));
			*/

			_ball.BallDied = OnBallDied;
			_ui.StartGame = OnStartGame;

			GameState.LoadHighScore();
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
			return (GameState.Level - 1) * 50.0f;
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
			pickupNode.ApplyImpulse(direction * 500.0f);
			pickupNode.ApplyTorqueImpulse(direction.X * 1800.0f);
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
			_ball.LaunchSpeed += GetLevelSpeedModifier();
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
