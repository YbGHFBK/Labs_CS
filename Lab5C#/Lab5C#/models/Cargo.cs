using System.Xml.Serialization;

[XmlRoot("Cargo")]
public class Cargo : Item
{
    public Cargo() { } 

    public Cargo(int weight) : base(weight) { }

}