using System.Xml.Serialization;

[XmlInclude(typeof(Passenger))]
[XmlInclude(typeof(Baggage))]
[XmlInclude(typeof(Cargo))]
public abstract class Item
{
    [XmlAttribute]
    public int weight {  get; set; }

    public Item() { }

    public Item(int weight)
    {
        this.weight = weight;
    }
}