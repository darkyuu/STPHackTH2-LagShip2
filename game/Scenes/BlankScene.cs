using Godot;
using System;

public class BlankScene : Node
{
	Label labelTest = null;
	
	Autoload globals;

    public override void _Ready()
    {
		this.globals = (Autoload)GetNode("/root/Autoload");
		
		labelTest = GetNode("Label1") as Label;
		labelTest.Text = "Hello"+this.globals.score;
		GD.Print("GetChildCount: "+GetChildCount());
		SetPhysicsProcess(true); //TUU use PhysicsProcess() insteads of FixProcess 
    }

	private void OnButtonPressed()
	{
		((Label)GetNode("Label1")).Text = "Hello";
		
	}


	
//	public override void _PhysicsProcess(float delta)
//	{
//		GD.Print("do _PhysicsProcess");
//	}

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}