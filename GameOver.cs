using Godot;
using System;

public partial class GameOver : CanvasLayer
{
	[Signal]
	public delegate void RestartEventHandler();
	
	public override void _Ready(){
		GetNode<Button>("RestartButton").Pressed+= _on_restart_button_pressed;
	}
	
	public void _on_restart_button_pressed(){
		EmitSignal(SignalName.Restart);
	}
}
