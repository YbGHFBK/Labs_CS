class Program
{

    static void Main(string[] args)
    {
        Game.inputFile = "TextFiles/ChaseData.txt";
        Game.outputFile = "TextFiles/PursuitLog.txt";

        Game game = new Game(16);
        game.Run();
    }
}