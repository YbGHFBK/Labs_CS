using System.Xml.Serialization;

[XmlRoot("PassengerCarriege")]
public class PassengerCarriege : Carriege
{
    [XmlAttribute]
    public int comfortLevel { get; set; }

    [XmlElement("Passenger", typeof(User))]
    public List<User> passengers = new();

    public PassengerCarriegeType type;

    public int freeSeats;




    public PassengerCarriege() { }

    public PassengerCarriege(int carryingCapacity, PassengerCarriegeType type) : base(carryingCapacity)
    {
        freeSeats = carryingCapacity;
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
        return freeSeats;
    }

    public int GetFreeSeatNumber()
    {
        return carryingCapacity - freeSeats + 1;
    }

    public override double GetTypeCostModifier()
    {
        switch (type)
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