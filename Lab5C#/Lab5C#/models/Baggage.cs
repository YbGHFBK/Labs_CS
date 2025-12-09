using System.Xml.Serialization;

[XmlRoot("Baggage")]
public class Baggage : Item
{
    public Baggage() { }

    public Baggage(int weight) : base(weight) { }
}