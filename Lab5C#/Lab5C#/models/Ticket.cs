using System.Xml.Serialization;

[XmlRoot("Ticket")]
public class Ticket : IHasId
{
    [XmlAttribute("Id")]
    public int Id {  get; set; }

    [XmlElement("Passenger")]
    public Passenger passenger;

    [XmlElement("Route")]
    public Route route;

    [XmlElement("Train")]
    public Train train;

    [XmlElement("Car")]
    public PassengerCarriege car;

    [XmlAttribute("Price")]
    public double price;

    [XmlAttribute("Seat")]
    public int seat;

    [XmlAttribute("PurchaseDate")]
    public DateTime purchaseDate;

    public Ticket() { }
    public Ticket(Passenger passenger, Route route, Train train, PassengerCarriege car, double price, int seat, List<Ticket> tickets)
    {
        this.passenger = passenger;
        this.route = route;
        this.train = train;
        this.car = car;
        this.price = price;
        this.seat = seat;
        Id = IdGenerator.GetNextId(tickets);
        purchaseDate = DateTime.Now;
    }
}