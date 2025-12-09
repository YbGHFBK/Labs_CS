using System.Xml.Serialization;

[XmlRoot("Locomotive")]
public class Locomotive : Carriege
{
    public Locomotive() { }

    public Locomotive(int carryingCapacity) : base(carryingCapacity) { }

    public override string ToString()
    {
        return base.ToString() + " вагонов";
    }

    public override string GetClass()
    {
        return "Locomotive\t";
    }
}