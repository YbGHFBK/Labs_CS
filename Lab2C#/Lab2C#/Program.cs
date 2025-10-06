class Program
{

    static void Main(string[] args)
    {
        Game.inputFile = "TextFiles/ChaseData.txt";
        Game.outputFile = "TextFiles/PursuitLog.txt";

        string[] text = File.ReadAllLines(Game.inputFile);

        Game.size = int.Parse(text[0]);

        Game game = new Game();
        game.Run(text);
    }
}