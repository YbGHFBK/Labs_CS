using System.Xml.Serialization;

[XmlRoot("Passenger")]
public class Passenger : Item
{
    public Passenger() { }

    public Passenger(int weight) : base(weight) { }
}