using System.Xml.Serialization;

[XmlRoot("PassengerCarriege")]
public class PassengerCarriege : Carriege
{
    [XmlAttribute]
    public int comfortLevel { get; set; }

    [XmlElement("Passenger", typeof(Passenger))]
    public List<Passenger> passengers = new();

    [XmlElement("Baggage", typeof(Baggage))]
    public List<Baggage> baggages = new();


    public Passenger[] seats = new Passenger[64];

    public PassengerCarriegeType type;




    public PassengerCarriege() { }

    public PassengerCarriege(int carryingCapacity, int comfortLevel, PassengerCarriegeType type) : base(carryingCapacity)
    {
        this.comfortLevel = comfortLevel;
        this.type = type;
    }




    public override string GetClass()
    {
        return "Passenger Carriege";
    }

    public override string ToString()
    {
        return base.ToString() + " человек" + " \t| comfort level: " + comfortLevel;
    }

    public void AddPassenger(Passenger passenger)
    {
        bool isSeated = false;
        for(int i = 0; i < seats.Length; i++)
        {
            if(seats[i] == null)
            {
                seats[i] = passenger;
                isSeated = true;
                break;
            }
        }

        if (!isSeated)
        {
            Console.WriteLine("В этом вагоне больше нет мест");
        }
        else
        {

        }
    }
}