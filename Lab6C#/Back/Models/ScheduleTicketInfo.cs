public class ScheduleTicketInfo
{
    public Schedule Schedule;
    public int CarNum;
    public double Price;

    public ScheduleTicketInfo(Schedule schedule, int carNum, double price)
    {
        Schedule = schedule;
        CarNum = carNum;
        Price = price;
    }

    public void CalcPrice()
    {
        double price = 0.00;

        Route route = DB.GetById<Route>(Schedule.RouteId);
        Train train = DB.GetById<Train>(Schedule.TrainId);
        PassengerCarriege car = (PassengerCarriege)train.carrieges[CarNum];

        price += route.distance;
        price *= GetPriceModifier(car.type);

        Price = price;
    }

    public double GetPriceModifier(PassengerCarriegeType type) => (double)type / 10.0;
}