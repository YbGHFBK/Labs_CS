using System.Text;
using System.Xml.Serialization;

[XmlRoot("Passenger")]
public class Passenger : Item, IHasId
{
    [XmlAttribute("ID")]
    public int Id { get; set; }
    [XmlAttribute("Login")]
    public string login;
    [XmlAttribute("Email")]
    public string email;
    [XmlAttribute("Password")]
    public string password;
    [XmlAttribute("Phone")]
    public string phone;
    [XmlAttribute("Role")]
    public UserRole role;

    [XmlArray("Tickets")]
    [XmlArrayItem("Ticket", typeof(Ticket))]
    public List<Ticket> tickets;
    public Passenger() { }

    public Passenger(string login, string password, List<Passenger> users)
    {
        this.login = login;
        this.password = password;
        IdGenerator.GetNextId(users);
    }
    public Passenger(int weight) : base(weight) { }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"login: {login}\n");
        sb.Append($"email: {(email == null ? "not specified" :  email)}\n");
        sb.Append($"phone: {(phone == null ? "not specified" :  phone)}\n");
        sb.Append($"role: {(role == UserRole.Admin ? "Admin" : "Regular")}");

        return sb.ToString();
    }

    public void AddTicket(Ticket ticket)
    {
        tickets.Add(ticket);
    }
}