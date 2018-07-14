using Godot;
using System;

public class LeftCommandObject : RigidBody2D
{
    public override void _Ready()
    {
		AddToGroup("commands");
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
