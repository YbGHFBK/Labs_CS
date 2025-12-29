using System.Text;
using System.Xml.Serialization;

public class Train : IHasId
{
    [XmlAttribute("Id")]
    public int Id { get; set; }

    [XmlArray("Carrieges")]
    [XmlArrayItem("Locomotive", typeof(Locomotive))]
    [XmlArrayItem("PassengerCarriege", typeof(PassengerCarriege))]
    [XmlArrayItem("CargoCarriege", typeof(CargoCarriege))]
    public List<Carriege> carrieges = new();

    [XmlAttribute("Type")]
    public TrainType type;

    [XmlAttribute("Condition")]
    public TrainCondition condition;

    [XmlAttribute("Model")]
    public string model;

    [XmlAttribute("ReleaseDate")]
    public DateTime releaseDate;

    [XmlAttribute("Mileage")]
    public int mileage;

    public Train()
    {
        carrieges = new List<Carriege>();
        Id = DB.GetNextId(this.GetType());
    }

    public void Add(Carriege carriege)
    {
        carrieges.Add(carriege);
    }

    public void Delete(int id)
    {
        carrieges.RemoveAt(id);
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append(model + " " + type.ToString());

        //foreach (Carriege car in carrieges)
        //{
        //    sb.Append('\n' + car.ToString());
        //}

        return sb.ToString();
    }

    public string SetType(TrainType type)
    {
        if (type == this.type) return "Поезд уже является " + (type == TrainType.Passenger ? "пассажирским" : "грузовым");

        bool hasWrongCars = false;

        foreach (Carriege car in carrieges)
        {
            if (car.GetCarType() != type)
            {
                hasWrongCars = true;
                break;
            }
        }

        if (hasWrongCars)
            return "Нельзя поменять тип поезда, пока он содержит вагоны другого типа";

        this.type = type;

        return "Тип поезда успешно изменён";
    }

    public void SetCondition(TrainCondition condition)
    {
        this.condition = condition;
    }

    public List<Carriege> GetCarsExceptLocos()
    {
        List<Carriege> cars = new();

        foreach (Carriege car in carrieges)
        {
            if (car is not Locomotive)
            {
                cars.Add(car);
            }
        }

        return cars;
    }
}