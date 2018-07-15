using Godot;
using System;

public class IntroScene : Node2D
{
    public override void _Ready()
    {

    }

	public void OnStartGameButtonPressed()
	{
		GetTree().ChangeScene("res://Scenes/GameplayScene.tscn");
	}
}



