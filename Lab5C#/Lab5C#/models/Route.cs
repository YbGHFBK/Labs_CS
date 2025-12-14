using System.Xml.Serialization;

[XmlRoot("Route")]
public class Route : IHasId
{
    [XmlElement]
    public Station routeStart;
    [XmlElement]
    public Station routeEnd;
    [XmlAttribute("ID")]
    public int Id {  get; set; }

    [XmlArray("Stations")]
    [XmlArrayItem("Station", typeof(Station))]
    public List<Station> stations = new();

    [XmlAttribute("Distance")]
    public int distance;
    [XmlAttribute("TravelTime")]
    public int travelTime;

    public Route() { }

    public Route(Station routeStart, Station routeEnd, List<Route> routes)
    {
        this.routeStart = routeStart;
        this.routeEnd = routeEnd;
        Id = IdGenerator.GetNextId(routes);
    }

    public Route(Station routeStart, Station routeEnd, int id)
    {
        this.routeStart = routeStart;
        this.routeEnd = routeEnd;
        Id = id;

        stations.Add(routeStart);
        stations.Add(routeEnd);
    }

    public Route(Station routeStart, Station routeEnd, List<Station> stations)
    {
        this.routeStart = routeStart;
        this.routeEnd = routeEnd;

        stations.Add(routeStart);

        this.stations.AddRange(stations);

        stations.Add(routeEnd);
    }

    public void AddStation(Station station)
    {
        stations.Add(station);
    }
}