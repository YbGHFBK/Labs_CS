using System.Xml.Serialization;

public class Schedule : IHasId
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("Route")]
    public int RouteId { get; set; }

    [XmlAttribute("Train")]
    public int TrainId { get; set; }

    [XmlIgnore]
    public TimeOnly DepartureDate { get; set; }

    [XmlElement("DepartureDate")]
    public string DepartureDateString
    {
        get => DepartureDate.ToString("HH:mm:ss");
        set => DepartureDate = TimeOnly.Parse(value);
    }

    [XmlIgnore]
    public TimeOnly ArrivalDate { get; set; }

    [XmlElement("ArrivalDate")]
    public string ArrivalDateString
    {
        get => ArrivalDate.ToString("HH:mm");
        set => ArrivalDate = TimeOnly.Parse(value);
    }

    [XmlIgnore]
    public TimeOnly Duration { get; set; }

    [XmlElement("Duration")]
    public string DurationString
    {
        get 
        {
            TimeSpan d = ArrivalDate - DepartureDate;
            return d.ToString();
        }
        set => DepartureDate = TimeOnly.Parse(value);
    }

    public Schedule()
    {
        Id = DB.GetNextId(this.GetType());
    }
}