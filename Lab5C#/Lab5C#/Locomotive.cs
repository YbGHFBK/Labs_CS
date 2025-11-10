public class Locomotive : Carriege
{
    public Locomotive() { }

    public Locomotive(int carryingCapacity) : base(carryingCapacity) { }

    public override string ToString()
    {
        return base.ToString();
    }

    public override string GetClass()
    {
        return "Locomotive\t";
    }
}