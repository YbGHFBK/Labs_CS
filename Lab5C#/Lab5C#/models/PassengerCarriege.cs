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


    public Passenger[] seats;

    public PassengerCarriegeType type;




    public PassengerCarriege() { }

    public PassengerCarriege(int carryingCapacity, PassengerCarriegeType type) : base(carryingCapacity)
    {
        seats = new Passenger[carryingCapacity];
        this.type = type;
    }

    public PassengerCarriege(int carryingCapacity, int comfortLevel, PassengerCarriegeType type) : base(carryingCapacity)
    {
        seats = new Passenger[carryingCapacity];
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

    public override string GetCarSpecType()
    {
        return type.ToString();
    }

    public override TrainType GetCarType()
    {
        return TrainType.Passenger;
    }

    public int GetFreeSeats()
    {
        int count = 0;

        for (int i = 0; i < carryingCapacity; i++)
        {
            try
            {
                if (seats[i] is Passenger)
                {
                    count++;
                }
            }
            catch { }
        }

        return carryingCapacity - count;
    }

    public int GetFreeSeatNumber()
    {
        bool isFree = false;

        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i] == null)
            {
                isFree = true;
                return i + 1;
            }
        }
        return -1;
    }

    public override double GetTypeCostModifier()
    {
        switch(type)
        {
            case PassengerCarriegeType.ReservedSeat:
                return 1.35;
            case PassengerCarriegeType.Seat:
                return 1.10;
            case PassengerCarriegeType.Compartment:
                return 1.90;
        }
        return -1.0;
    }
}