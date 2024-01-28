using Godot;
using System;

public partial class UI : Control
{
    private Label _scoreLabel;
    private Label _livesLabel;

    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _livesLabel = GetNode<Label>("LivesLabel");
    }

    public void UpdateScore(int newScore)
    {
        _scoreLabel.Text = "Score: " + newScore.ToString();
    }

    public void UpdateLives(int newLives)
    {
        _livesLabel.Text = "Lives: " + newLives.ToString();
    }
}
