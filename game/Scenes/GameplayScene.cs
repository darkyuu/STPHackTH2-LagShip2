using Godot;
using System;

public class GameplayScene : Node
{	
	[Export]
    public PackedScene rightCommands;
    [Export]
    public PackedScene leftCommands;
	[Export]
    public PackedScene reefs;
	[Export]
    public int maxCommandBuffer = 7;
	[Export]
    public int maxCommandFrameCounter = 10;
	[Export]
	public int distanceFromPlayerShipPoint = 290;
	[Export]
	public int distanceFromGoal = 1852;

	private ShipObject ship;
	private Position2D commandStartPoint;
	private CommandExecutionPointObject commandExecutionPoint;
	private Timer reefSpawnTimer;
	private Node reefPool;
	private Label remainingNMileLabel;
	private Label messageLabel;
	private Godot.Timer messageTimer;
	
	private int currentCommandFrameCounter;
	private float velocity;
	private int weatherFactor = 5; // value 0 to 9
	private double aimToPosition = 3*Math.PI/2;

	private float[] commandVelocity = new float[] {
		1600, 1600, 1400, 1400, 1000, 1000, 800, 800, 800, 800
	};
	
	private float commandMinimumVelocity = 300;

	private float[] commandLatencyFactor = new float[] {
		0.5f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 4.5f, 5.0f
	};
	
	private int difficult = 0;
	private int[] remainingNMileLevels = new int[] {
		1500*60, 1000*60, 750*60, 500*60
	};
	
	private float[] reefSpawnWaitTimeLevels = new float[] {
		3.5f, 3.3f, 3, 2.5f, 2
	};
	
	Autoload globals;

    public override void _Ready()
    {
        ship = GetNode("PlayerShip") as ShipObject;
        Position2D temp = GetNode("PlayerShipPoints/2") as Position2D;
		commandStartPoint = GetNode("CommandPanel/CommandStartPoint") as Position2D;
		commandExecutionPoint = GetNode("CommandPanel/CommandExecutionPoint") as CommandExecutionPointObject;
		reefSpawnTimer = GetNode("ReefSpawnTimer") as Timer;
		reefPool = GetNode("ReefPool") as Node;
		remainingNMileLabel = GetNode("CommandPanel/RemainingNMileLabel") as Label;
		messageLabel = GetNode("MessageLabel") as Label;
		messageTimer = GetNode("MessageTimer") as Godot.Timer;
		
		this.globals = (Autoload)GetNode("/root/Autoload");

		this.globals.Randomize();
        ship.SetStartPosition(temp.Position, Int32.Parse(temp.Name));
		velocity = commandVelocity[weatherFactor];
		reefSpawnTimer.Start();
		distanceFromGoal *= 60; //multiply with 60 because frame-per-sec principle
		this.globals.missionComplete = false;
		difficult = 0;
		messageLabel.Hide();
		messageTimer.Stop();
		
		commandExecutionPoint.Connect("CallRight",this, "ForceShipTurnRight");
        commandExecutionPoint.Connect("CallLeft",this, "ForceShipTurnLeft");
		ship.Connect("Hit", this, "DoGameOver");
    }

    public override void _Process(float delta)
    {
        if(currentCommandFrameCounter == maxCommandFrameCounter &&
			!this.globals.missionComplete)
		{
            InputFromPlayer();
		}
        else
            currentCommandFrameCounter += 1;
		
		if(!this.globals.missionComplete)
			ProcessDistanceFromGoal();
    }
	
	private void ProcessDistanceFromGoal()
	{
		distanceFromGoal--;
		if(distanceFromGoal == 0)
			DoMissionComplete();
		else
		{
			if(difficult < remainingNMileLevels.Length)
				if(distanceFromGoal == remainingNMileLevels[difficult])
				{
					difficult++;
					reefSpawnTimer.WaitTime = reefSpawnWaitTimeLevels[difficult];
				}
		}
		remainingNMileLabel.Text = string.Format("{0:0.00}", distanceFromGoal/1000.0);
	}
	
	private void InputFromPlayer()
    {
        if(this.globals.currentCommandBuffer < maxCommandBuffer)
        {
			if(Input.IsActionJustPressed("ui_left"))
	            CreateCommand("left");
	        else if(Input.IsActionJustPressed("ui_right"))
	            CreateCommand("right");
		}
	}

    public void CreateCommand(String command)
    {
		RigidBody2D commandObj = null;
        if(command.Equals("left"))
            commandObj = (RigidBody2D)leftCommands.Instance();
        else if(command.Equals("right"))
            commandObj = (RigidBody2D)rightCommands.Instance();

        if(commandObj != null)
        {
            AddChild(commandObj);
            float direction = 0;
            commandObj.Position = commandStartPoint.Position;
			velocity = commandVelocity[weatherFactor];
            int baseValue = 5;
            velocity -= commandLatencyFactor[weatherFactor]/baseValue*velocity;
            velocity = Mathf.Clamp(velocity, commandMinimumVelocity, commandVelocity[0]);

            commandObj.SetLinearVelocity(new Vector2(velocity, 0).Rotated(direction));

            this.globals.currentCommandBuffer += 1;
            currentCommandFrameCounter = 0;
        }
    }
	
	private void ForceShipTurnLeft()
    {
		switch(ship.GetCurrentLaneIndex())
        {
            case 1: /*show bad command signal*/ break;
            case 2:
            case 3: 
            {
				int targetIndex = ship.GetCurrentLaneIndex()-1;
				Position2D temp = GetNode("PlayerShipPoints/"+targetIndex.ToString()) as Position2D;
				ship.Move(temp.Position, targetIndex);
                break;
            }
        }
    }

    private void ForceShipTurnRight()
    {
		switch(ship.GetCurrentLaneIndex())
        {
            case 1:
            case 2:
            {
                int targetIndex = ship.GetCurrentLaneIndex()+1;
                Position2D temp = GetNode("PlayerShipPoints/"+targetIndex.ToString()) as Position2D;
				ship.Move(temp.Position, targetIndex);
                break;
            }
            case 3: /*show bad command signal*/ break;
        }
    }
	
	private void OnReefSpawnTimerTimeout()
	{
		int tempSpawnStartIndex = this.globals.randomGenerator.Next() % 3 + 1;

		SpawnReef(tempSpawnStartIndex);
	}
	
	private void SpawnReef(int spawnStartIndex)
	{
		Vector2 tempStartPosition = ((Position2D)GetNode("PlayerShipPoints/"+spawnStartIndex.ToString())).GetPosition();
        tempStartPosition.y = tempStartPosition.y + distanceFromPlayerShipPoint;
		
        ReefObject obj = (ReefObject)reefs.Instance();
        reefPool.AddChild(obj);
        obj.Position = tempStartPosition;
        obj.Rotation = 0;
        obj.SetLinearVelocity(new Vector2(70, 0).Rotated((float)aimToPosition));
	}
	
	private void ShowMessage(string textString)
    {
        messageLabel.Text = textString;
        messageLabel.Show();
        messageTimer.Start();
    }
	
	public void DoMissionComplete()
    {
		this.globals.missionComplete = true;
        ShowMessage("Mission\nComplete");
		GoBackToMainMenuAfterMessageIsGone();
    }
	
	public void DoGameOver()
    {
		this.globals.missionComplete = true;
        ShowMessage("Game Over");
		GoBackToMainMenuAfterMessageIsGone();
    }
	
	private async void GoBackToMainMenuAfterMessageIsGone()
    {
        await ToSignal(messageTimer, "timeout");
		GetTree().ChangeScene("res://Scenes/IntroScene.tscn");
    }

}



