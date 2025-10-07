using System.Text;

enum GameState
{
    Start,
    End
}

class Game
{
    public static string inputFile;
    public static string outputFile;

    public static int size;
    public Player cat;
    public Player mouse;
    public GameState state;

    private StringBuilder sb = new StringBuilder();

    public Game()
    {
        cat = new Player("Cat");
        mouse = new Player("Mouse");
        state = GameState.Start;
    }

    public void Run(string[] text)
    {
        SbAppend("Cat and Mouse\n\nCat\tMouse\tDistance\n" + new string('-', 20) + "\n");

        int counter = 1;

        Draw(true);

        while (state != GameState.End)
        {
            string[] parts = text[counter++].Split('\t');

            switch (parts[0])
            {
                case "M":
                    DoCommand(mouse, int.Parse(parts[1]));
                    GameCheck();
                    break;

                case "C":

                    DoCommand(cat, int.Parse(parts[1]));
                    GameCheck();
                    break;

                case "P":
                    SbAppend();
                    break;

                default:
                    break;
            }

            if (counter == text.Length - 1) state = GameState.End;

        }

        SbAppend(new string('-', 20) + "\n\n\nDistance traveled:\tCat\tMouse\n\t\t\t\t\t" + cat.distanceTravelled + "\t" + mouse.distanceTravelled);
        if (cat.state == PlayerState.Winner) SbAppend("\nMouse caught at: " + mouse.location);
        else SbAppend("\nMouse evaded Cat");

        DoPrint();
    }

    private void DoCommand(Player player, int steps)
    {
        player.Move(steps);

        Draw();
        Console.WriteLine(cat.location + "\t" + mouse.location + "\t" + FindDistance());
    }

    private bool InBounds(int location)
    {
        if(location <= 0 || location > size) return false;
        return true;
    }

    private void GameCheck()
    {
        if (mouse.location == cat.location)
        {
            cat.state = PlayerState.Winner;
            mouse.state = PlayerState.Looser;
            state = GameState.End;
        }
    }

    private int FindDistance()
    {
        if (!InBounds(mouse.location) || !InBounds(cat.location)) return -1; 
        return Math.Abs(cat.location - mouse.location);
    }

    public void SbAppend(string text = null)
    {
        if (text != null) sb.Append(text);
        else
        {
            sb.Append(cat.GetLocation() + "\t" + mouse.GetLocation() + "\t\t" + FindDistance() + "\n");
        }
    }

    private void DoPrint()
    {
        using (StreamWriter sw = new StreamWriter(outputFile, false))
        {
            try
            {
                sw.WriteLine(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка записи в файл:\n");
                throw;
            }
        }
    }

    public void Draw(bool start = false)
    {
        if (start)
        {
            for (int i = 1; i <= size; i++)
            {
                if (i / 10 >= 1) Console.Write(" " + i);
                else Console.Write(" " + i + " ");
            }
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < size; i++)
        {
            if (cat.location == (i + 1))
            {
                Console.Write("|C ");
            }
            else if (mouse.location == (i + 1))
            {
                Console.Write("|M ");
            }
            else Console.Write("|  ");
        }
        Console.Write('|');
    }

}
