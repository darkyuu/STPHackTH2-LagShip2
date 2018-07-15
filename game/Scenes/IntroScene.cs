using Godot;
using System;

public class IntroScene : Node2D
{
	private AudioStreamPlayer bgm;
    public override void _Ready()
    {
		bgm = GetNode("BGM") as AudioStreamPlayer;
		bgm.Stop();
		bgm.Play();
    }

	public void OnStartGameButtonPressed()
	{
		bgm.Stop();
		GetTree().ChangeScene("res://Scenes/GameplayScene.tscn");
	}
}



