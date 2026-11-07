using Godot;
using System;

public partial class Pipe : Area2D
{
	[Signal]
	public delegate void HitEventHandler();
	
	[Signal]
	public delegate void ScoreEventHandler();

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Bird)
		{
			EmitSignal(SignalName.Hit);
		}
	}
	
	private void _on_score_area_body_entered(Node2D body){
		if (body is Bird)
		{
			EmitSignal(SignalName.Score);
		}
	}
}
