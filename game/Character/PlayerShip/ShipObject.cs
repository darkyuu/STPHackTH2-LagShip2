using Godot;
using System;

public enum ShipMovementStatus
{
    IDLE = 0,
    MOVE
};

public class ShipObject : Area2D
{   

	private float turnLeftAngle = (float)Math.PI/4;
	private float turnRightAngle = -(float)Math.PI/4;

    public ShipMovementStatus movementStatus;
    public int currentLaneIndex = 1;
    private Vector2 startPosition = new Vector2();
	private Vector2 targetPosition = new Vector2();

    public override void _Ready()
    {
        movementStatus = ShipMovementStatus.MOVE;
        SetProcess(false);
    }

    public override void _Process(float delta)
    {
        if(movementStatus == ShipMovementStatus.IDLE)
        {
            if (Input.IsActionJustPressed("ui_right"))
                SetRotation(turnRightAngle);
            if (Input.IsActionJustPressed("ui_left"))
                SetRotation(turnLeftAngle);
        }
    }

    public void SetStartPosition(Vector2 pos, int laneIndexVal)
    {
        this.currentLaneIndex = laneIndexVal;
        this.startPosition.x = pos.x;
        this.startPosition.y = pos.y;
        SetPosition(startPosition);
    }
	
	public void MoveTo()
	{
		
//		if(commandString.Equals("left"))
//        {
//            switch(ship.currentLaneIndex)
//            {
//                case 1: /*show bad command signal*/ break;
//                case 2:
//                case 3: 
//                {
//                    int targetIndex = ship.currentLaneIndex-1;
//                    Position2D temp = GetNode("PlayerShipPoints/"+targetIndex.ToString()) as Position2D;
//                    ship.Position = temp.Position;
//                    ship.currentLaneIndex = targetIndex;
//                    break;
//                }
//            }
//        }
//        else if(commandString.Equals("right"))
//        {
//            switch(ship.currentLaneIndex)
//            {
//                case 1:
//                case 2:
//                {
//                    int targetIndex = ship.currentLaneIndex+1;
//                    Position2D temp = GetNode("PlayerShipPoints/"+targetIndex.ToString()) as Position2D;
//                    ship.Position = temp.Position;
//                    ship.currentLaneIndex = targetIndex;
//                    break;
//                }
//                case 3: /*show bad command signal*/ break;
//            }
//        }
	}
}
