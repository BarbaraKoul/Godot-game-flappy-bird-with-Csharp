using Godot;
using System;

public partial class Bird : CharacterBody2D
{
	private const int GRAVITY = 1000;
	private const int MAX_VEL = 600;
	private const int FLAP_SPEED=-500;
	public bool flying = false;
	public bool falling = false;
	private Vector2 START_POS = new Vector2(100,400);
	
	public override void _Ready()
	{
		Reset();
	}

	public override void _Process(double delta)
	{
	}
	
	public void Reset()
	{
		falling = false;
		flying = false;
		Position = START_POS;
		Rotation=0f;	
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 v = Velocity;
		if(flying || falling){
			v.Y +=GRAVITY*(float)delta;
			if(v.Y>MAX_VEL){
				v.Y = MAX_VEL;
			}
			Velocity = v;
			if(flying){
				Rotation = Mathf.DegToRad(Velocity.Y*0.05f);
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play();
			}
			else if(falling){
				Rotation = Mathf.Pi/2f;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
			}
			MoveAndCollide(Velocity * (float)delta);
		}
		else{
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
		}
	}
	
	public void flap(){
		Vector2 v = Velocity;
		v.Y=FLAP_SPEED;
		Velocity = v;
	}
}
