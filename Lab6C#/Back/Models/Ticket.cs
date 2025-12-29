using System.Xml.Serialization;

public class Ticket : IHasId
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlAttribute("User")]
    public int UserId { get; set; }

    [XmlAttribute("Car")]
    public int CarNum { get; set; }

    [XmlAttribute("Price")]
    public double Price { get; set; }

    [XmlAttribute("Seat")]
    public int Seat { get; set; }

    [XmlAttribute("PurchaseDate")]
    public TimeOnly PurchaseDate { get; set; }

    public Ticket()
    {
        Id = DB.GetNextId(this.GetType());
    }
}