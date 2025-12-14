using System.Xml.Serialization;

[XmlRoot("Station")]
public class Station : IHasId
{
    [XmlAttribute("Country")]
    public string country;
    [XmlAttribute("City")]  
    public string city;
    [XmlAttribute("Id")]
    public int Id { get; set;  }

    public Station() { }

    public Station(string country, string city, List<Station> stations)
    {
        this.country = country;
        this.city = city;
        Id = IdGenerator.GetNextId(stations);
    }

    public Station(string country, string city, int id)
    {
        this.country = country;
        this.city = city;
        Id = id;
    }

}