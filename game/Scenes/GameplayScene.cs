using Godot;
using System;

public class GameplayScene : Node
{
	[Export]
    public PackedScene rightCommands;
    [Export]
    public PackedScene leftCommands;
	[Export]
    public int maxCommandBuffer = 7;
	[Export]
    public int maxCommandFrameCounter = 10;

	private ShipObject ship;
	private Position2D commandStartPoint;
	
	private int currentCommandFrameCounter;
	private float velocity;
	private int weatherFactor = 5; // value 0 to 9
	private float[] commandVelocity = new float[]{
		1600, 1600, 1400, 1400, 1000, 1000, 800, 800, 800, 800
	};
	private float commandMinimumVelocity = 300;

	private float[] commandLatencyFactor = new float[]{
		0.5f, 1.0f, 1.5f, 2.0f, 2.5f, 3.0f, 3.5f, 4.0f, 4.5f, 5.0f
	};
	
	Autoload globals;

    public override void _Ready()
    {
        ship = GetNode("PlayerShip") as ShipObject;
        Position2D temp = GetNode("PlayerShipPoints/2") as Position2D;
		commandStartPoint = GetNode("CommandPanel/CommandStartPoint") as Position2D;
		
		this.globals = (Autoload)GetNode("/root/Autoload");

        ship.SetStartPosition(temp.Position, Int32.Parse(temp.Name));
		velocity = commandVelocity[weatherFactor];
    }

    public override void _Process(float delta)
    {
        if(currentCommandFrameCounter == maxCommandFrameCounter)
            InputFromPlayer();
        else
            currentCommandFrameCounter += 1;
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

}
