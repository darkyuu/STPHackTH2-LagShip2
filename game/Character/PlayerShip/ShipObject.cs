using Godot;
using System;

public class ShipObject : Area2D
{   

	private float turnLeftAngle = (float)Math.PI/4;
	private float turnRightAngle = -(float)Math.PI/4;

    private int currentLaneIndex = 1;
    private Vector2 startPosition = new Vector2();
	private Vector2 targetPosition = new Vector2();

    public override void _Ready()
    {
        SetProcess(false);
    }

    public override void _Process(float delta)
    {
        
    }

    public void SetStartPosition(Vector2 pos, int laneIndexVal)
    {
		this.currentLaneIndex = laneIndexVal;
		this.startPosition.x = pos.x;
		this.startPosition.y = pos.y;
		SetPosition(startPosition);
    }
	
	public int GetCurrentLaneIndex()
	{
		return currentLaneIndex;
	}
	
	public void Move(Vector2 targetPos, int targetLaneIndex)
	{
		this.Position = targetPos;
		this.currentLaneIndex = targetLaneIndex;
	}
}
