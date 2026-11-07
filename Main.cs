using Godot;
using System;
using System.Collections.Generic;

public partial class Main : Node
{
	[Export]
	public PackedScene PipeScene {get; set;}
	
	private bool gameRunning;
	private bool gameOver;
	private float scroll;
	private int score;
	private const int SCROLL_SPEED=4;
	private Vector2I screenSize;
	private int groundHeight;
	private List<Area2D> pipes = new List<Area2D>();
	private const int PIPE_DELAY=100;
	private const int PIPE_RANGE=50;
	
	public override void _Ready()
	{
		screenSize = (Vector2I)GetViewport().GetVisibleRect().Size;
		groundHeight = (int)GetNode<Sprite2D>("Ground/Sprite2D").Texture.GetHeight();
		NewGame();
	}

	public override void _Process(double delta)
	{
		if(gameRunning){
			scroll+=SCROLL_SPEED;
			if(scroll > screenSize.X){
				scroll =0;
			}
			Area2D ground = GetNode<Ground>("Ground");
			Vector2 pos = ground.Position;
			pos.X = -scroll;
			ground.Position = pos;
			foreach (var pipe in pipes)
				{
					Vector2 pipePos = pipe.Position;
					pipePos.X -= SCROLL_SPEED;
					pipe.Position = pipePos;
				}
		}
	}
	
	public void NewGame()
	{
		gameRunning = false;
		gameOver = false;
		score = 0;
		scroll = 0;
		GetNode<Label>("ScoreLabel").Text = "SCORE: " + score;
		GetNode<CanvasLayer>("GameOver").Hide();
		foreach (var pipe in pipes)
		{
			pipe.QueueFree();
		}
		pipes.Clear();
		GeneratePipes();
		GetNode<Bird>("Bird").Reset();
	}
	
	public override void _Input(InputEvent @event)
	{
		if(!gameOver){
			if(@event is InputEventMouseButton mouseEvent){
				if(mouseEvent.ButtonIndex==MouseButton.Left && mouseEvent.Pressed){
					if(!gameRunning){
						StartGame();
					}
					else{
						Bird bird = GetNode<Bird>("Bird");
						if(bird.flying){
							bird.flap();
							CheckTop();
						}
					}
				}
			}
		}
	}
	
	public void StartGame()
	{
		gameRunning = true;
		Bird bird =GetNode<Bird>("Bird");
		bird.flying = true;
		bird.flap();
		GetNode<Timer>("PipeTimer").Start();
	}
	
	public void _on_pipe_timer_timeout()
	{
		GeneratePipes();
	}
	
	public void GeneratePipes()
{
	Area2D pipe = PipeScene.Instantiate<Area2D>();
	Vector2 pos = pipe.Position;

	pos.X = screenSize.X + PIPE_DELAY;
	pos.Y = (screenSize.Y - groundHeight) / 2f + (float)GD.RandRange(-PIPE_RANGE, PIPE_RANGE);

	pipe.Position = pos;
	pipe.Connect(Pipe.SignalName.Hit, Callable.From(BirdHit));
	pipe.Connect(Pipe.SignalName.Score, Callable.From(Scored));
	AddChild(pipe);
	pipes.Add(pipe);
}
	
	public void Scored(){
		score++;
		GetNode<Label>("ScoreLabel").Text = "SCORE: " + score;
	}
	
	public void BirdHit()
	{
		Bird bird = GetNode<Bird>("Bird");
		bird.falling = true;
		StopGame();
	}
	
	public void CheckTop()
	{
		Bird bird = GetNode<Bird>("Bird");
		if(bird.Position.Y<0){
			bird.falling = true;
			StopGame();
		}
	}
	
	public void StopGame(){
		GetNode<Timer>("PipeTimer").Stop();
		GetNode<CanvasLayer>("GameOver").Show();
		Bird bird = GetNode<Bird>("Bird");
		bird.flying = false;
		
		gameRunning = false;
		gameOver = true;
	}
	
	public void _on_ground_hit(){
		Bird bird = GetNode<Bird>("Bird");
		bird.falling = false;
		StopGame();
	}
	
	public void _on_game_over_restart(){
		NewGame();
	}
}
