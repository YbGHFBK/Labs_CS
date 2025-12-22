using System.Xml.Serialization;

[XmlRoot("Station")]
public class Station : IHasId, IComparable<Station>
{
    [XmlAttribute("Country")]
    public string country;
    [XmlAttribute("City")]
    public string city;
    [XmlAttribute("Id")]
    public int Id { get; set; }

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

    public override string ToString()
    {
        return $"{country}, {city} - {Id}";
    }

    public int CompareTo(Station? other)
    {
        if (other == null) return 1;

        int countryCompare = string.Compare(this.country, other.country, StringComparison.OrdinalIgnoreCase);
        if (countryCompare != 0) return countryCompare;

        return string.Compare(this.city, other.city, StringComparison.OrdinalIgnoreCase);
    }
}