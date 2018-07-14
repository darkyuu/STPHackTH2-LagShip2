using Godot;
using System;

public class CommandExecutionPointObject : Area2D
{
	[Signal]
    public delegate void CallRight();
    [Signal]
    public delegate void CallLeft();
	
	Autoload globals;

    public override void _Ready()
    {
		this.globals = (Autoload)GetNode("/root/Autoload");
		
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
        }
        else if(body.GetName().Find("LeftCommand") != -1)
        {
            this.EmitSignal("CallLeft");
        }

        this.globals.currentCommandBuffer -= 1;
        body.QueueFree();
	}
}



