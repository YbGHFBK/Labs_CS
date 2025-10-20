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

    public bool isStartsWithConsonant()
    {
        string consonants = "бвгджзклмнпрстфхцчшщbcdfghjklmnpqrstvwxz";
        if (consonants.Contains(letters.ToLower()[0])) return true;
        return false;
    }
}