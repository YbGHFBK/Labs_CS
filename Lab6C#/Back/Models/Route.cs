using System.Xml.Serialization;

[XmlRoot("Route")]
public class Route : IHasId, IComparable<Route>
{
    [XmlElement]
    public Station routeStart;
    [XmlElement]
    public Station routeEnd;

    [XmlElement("Start")]
    public int routeStartId;
    [XmlElement("End")]
    public int routeEndId;
    [XmlAttribute("ID")]
    public int Id { get; set; }

    [XmlAttribute("Distance")]
    public int distance;

    public Route()
    {
        Id = DB.GetNextId(this.GetType());
    }

    public int CompareTo(Route? other)
    {
        if (other == null) return 1;

        int countryCompare = this.routeStart.CompareTo(other.routeStart);
        if (countryCompare != 0) return countryCompare;

        return routeEnd.CompareTo(other.routeEnd);
    }

    public override string ToString()
    {
        return routeStart.city + "-" + routeEnd.city;
    }
}