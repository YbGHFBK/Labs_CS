enum GameState
{
    Start,
    End
}

class Game
{
    public static string inputFile;
    public static string outputFile;

    public int size;
    public Player cat;
    public Player mouse;
    public GameState state;

    public Game(int size)
    {
        this.size = size;
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.Start;
    }

    public void Run()
    {
        while (state != GameState.End)
        {

        }
    }

    private void DoCommand(char command, int steps)
    {
        switch(command)
        {
            case 'M': 
                mouse.Move(steps);
                if (!InBounds(mouse.location)) mouse.MoveToCorner(size);
                break;

            case 'C': 
                cat.Move(steps);
                if (!InBounds(cat.location)) cat.MoveToCorner(size);
                break;

            case 'P':
                break;
        }
    }

    private bool InBounds(int location)
    {
        if(location <= 0 && location > size) return false;
        return true;
    }

}
