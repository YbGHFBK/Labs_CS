using System.Drawing;

enum PlayerState
{
    Winner,
    Looser,
    InGame,
    NotInGame
}

class Player
{
    public string name;
    public int location;
    public PlayerState state = PlayerState.NotInGame;
    public int distanceTravelled = 0;

    public Player(string name)
    {
        state = PlayerState.NotInGame;
        this.name = name;
        this.location = -1;
    }

    public void Move(int steps)
    {
        if (state == PlayerState.InGame)
        {
            if (!InBounds(location + steps)) MoveToCorner(steps);
            else location += steps;
            distanceTravelled += Math.Abs(steps);
        }
        else if (state == PlayerState.NotInGame)
        {
            location = steps;
            state = PlayerState.InGame;
        }
    }
    
    public void MoveToCorner(int steps)
    {
        double count = steps / Game.size;
        steps = steps - ( (int)Math.Floor(count) * Game.size );
        if (location + steps > Game.size) location = steps - (Game.size - location);
        else if (location + steps <= 0) location = Game.size - (Math.Abs(steps) - location);
        else Move(steps);
    }

    public bool InBounds(int location)
    {
        if (location <= 0 || location > Game.size) return false;
        return true;
    }

    public string GetLocation()
    {
        if (location != -1) return location.ToString();
        return "??";
    }
}