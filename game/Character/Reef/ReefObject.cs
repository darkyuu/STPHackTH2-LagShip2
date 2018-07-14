using Godot;
using System;

public class ReefObject : RigidBody2D
{
    private Sprite sprite;
    private CollisionShape2D collision;
	
	private Vector2 velocity;
    private int rotationSpeed;
    private Vector2 extents;
	private int outOfFramePositionY = -32;
	
	Autoload globals;
	
    public override void _Ready()
    {
		sprite = GetNode("Sprite") as Sprite;
        collision = GetNode("CollisionShape2D") as CollisionShape2D;
		
		this.globals = (Autoload)GetNode("/root/Autoload");
		
		AddToGroup("reefs");
    }
	
	public override void _Process(float delta)
	{
		if(this.globals.missionComplete)
		{
			SetProcess(false);
			QueueFree();
		}
		
		if(Position.y <= outOfFramePositionY)
			QueueFree();
	}
	
	private void OnReefBodyEntered(Godot.Node body)
	{
		GD.Print("Reef hit");
		if(body.IsInGroup("commands"))
        {
			GD.Print("was hit by commands");
        }
	}
}



