using Godot;
using System;

public class Autoload : Node
{
	public int score = 0;
	public int currentCommandBuffer = 0;
	public Random randomGenerator = null;
	public bool missionComplete = false;
	
	public void Randomize()
	{
		if(randomGenerator == null)
			randomGenerator = new Random();
	}

}
