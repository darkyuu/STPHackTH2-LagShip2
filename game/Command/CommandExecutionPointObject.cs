using Godot;
using System;

public class CommandExecutionPointObject : Area2D
{
	[Signal]
    public delegate void CallRight();
    [Signal]
    public delegate void CallLeft();
	
	Autoload globals;
	
	private AudioStreamPlayer hitByCommandSound;

    public override void _Ready()
    {
		hitByCommandSound = GetNode("HitByCommandSound") as AudioStreamPlayer;
		
		this.globals = (Autoload)GetNode("/root/Autoload");
		
		hitByCommandSound.Stop();
		
		AddToGroup("commands");
		SetProcess(true);
    }

    public override void _Process(float delta)
    {
        
    }
	
	private void OnCommandExecutionPointBodyEntered(RigidBody2D body)
	{
        if(body.GetName().Find("RightCommand") != -1)
        {
            this.EmitSignal("CallRight");
			hitByCommandSound.Play();
        }
        else if(body.GetName().Find("LeftCommand") != -1)
        {
            this.EmitSignal("CallLeft");
			hitByCommandSound.Play();
        }

        this.globals.currentCommandBuffer -= 1;
        body.QueueFree();
	}
}



