using Godot;
using System;

public partial class Ground : Area2D
{
	[Signal]
	public delegate void HitEventHandler();

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
}
