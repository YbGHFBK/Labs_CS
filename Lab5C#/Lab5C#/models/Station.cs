using System.Xml.Serialization;

[XmlRoot("Station")]
public class Station
{
    [XmlAttribute("Country")]
    public string country;
    [XmlAttribute("City")]
    public string city;
    [XmlAttribute("Id")]
    public int id;

    public Station() { }

    public Station(string country, string city, int id)
    {
        this.country = country;
        this.city = city;
        this.id = id;
    }
}