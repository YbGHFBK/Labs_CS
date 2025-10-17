public class Word : Token
{
    public string letters;

    public Word(string letters)
    {
        this.letters = letters;
    }

    public override string ToString()
    {
        return letters;
    }
}