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
        int counter = 1;
        string[] text = File.ReadAllLines("TextFiles/ChaseData.txt");

        while (state != GameState.End)
        {
            string[] parts = text[counter++].Split('\t');

            switch (parts[0])
            {
                case "M":
                    DoCommand("M", int.Parse(parts[1]));
                    GameCheck();
                    break;

                case "C":

                    DoCommand("C", int.Parse(parts[1]));
                    GameCheck();
                    break;

                case "P":
                    DoPrint();
                    break;

                default:
                    break;
            }

        }
    }

    private void DoCommand(string command, int steps)
    {
        switch(command)
        {
            case "M":
                mouse.Move(steps);

                Draw();
                Console.WriteLine(cat.location + "\t" + mouse.location + "\t" + FindDistance() );

                if (!InBounds(mouse.location)) mouse.MoveToCorner(size);

                break;

            case "C":
                cat.Move(steps);

                Draw();
                Console.WriteLine(cat.location + "\t" + mouse.location + "\t" + FindDistance() );

                if (!InBounds(cat.location)) cat.MoveToCorner(size);

                break;
        }
    }

    private bool InBounds(int location)
    {
        if(location <= 0 && location > size) return false;
        return true;
    }

    private void GameCheck()
    {
        if (mouse.location == cat.location) state = GameState.End;
    }

    private int FindDistance()
    {
        if (!InBounds(mouse.location) || !InBounds(cat.location)) return -1; 
        return Math.Abs(cat.location - mouse.location);
    }

    public void DoPrint()
    {
        //Console.WriteLine(cat.location + '\t' + mouse.location + '\t' + FindDistance());
    }

    public void Draw()
    {
        for (int i = 0; i < size; i++)
        {
            if (cat.location == (i + 1))
            {
                Console.Write("C ");
            }
            else if (mouse.location == (i + 1))
            {
                Console.Write("M ");
            }
            else Console.Write("██");
        }
    }

}
