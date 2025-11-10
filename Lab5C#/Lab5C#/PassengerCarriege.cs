public class PassengerCarriege : Carriege
{
    int comfortLevel { get; set; }
    List<Passenger> passengers = new();
    List<Baggage> baggages = new();

    int Items => passengers.Count + baggages.Count;

    public PassengerCarriege() { }

    public PassengerCarriege(int carryingCapacity, int comfortLevel) : base(carryingCapacity)
    {
        this.comfortLevel = comfortLevel;
    }

    public override string GetClass()
    {
        return "Passenger Carriege";
    }

    public override string ToString()
    {
        return base.ToString() + " \t| comfort level: " + comfortLevel;
    }

    public override void Add(Item item)
    {
        if (item is Passenger)
            passengers.Add((Passenger)item);
        else if(item is  Baggage)
            baggages.Add((Baggage)item);
    }
}