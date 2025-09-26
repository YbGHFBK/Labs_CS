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
        this.name = name;
        this.location = -1;
    }

    public void Move(int steps)
    {
        if (state == PlayerState.InGame)
        {
            location += steps;
            distanceTravelled += Math.Abs(steps);
        }
        else if(state == PlayerState.NotInGame) location = steps;
    }
    
    public void MoveToCorner(int rightCorner)
    {
        if (location <= 0) location = rightCorner;
        else location = 1;
    }
}