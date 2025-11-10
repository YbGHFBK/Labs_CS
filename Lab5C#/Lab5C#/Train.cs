using System.Text;

public class Train
{
    public List<Carriege> carrieges = new();

    public Train() { }

    public void Add(Carriege carriege)
    {
        carrieges.Add(carriege);
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        foreach(Carriege car in carrieges)
        {
            sb.Append(car.ToString() + '\n');
        }

        return sb.ToString();
    }
}