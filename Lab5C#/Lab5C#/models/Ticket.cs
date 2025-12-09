public class Ticket
{
    public int id;

    public Passenger passenger;

    public float price;

    public Route route;

    public PassengerCarriege car;

    public int seat;


    public Ticket(int id, Passenger passenger, Route route, PassengerCarriege car)
    {
        this.id = id;
        this.passenger = passenger;
        this.route = route;
        this.car = car;
    }
}