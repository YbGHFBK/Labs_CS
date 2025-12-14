using System.Text;
using System.Xml.Serialization;

[XmlRoot("Train")]
public class Train : IHasId
{
    [XmlArray("Carrieges")]
    [XmlArrayItem("Locomotive", typeof(Locomotive))]
    [XmlArrayItem("PassengerCarriege", typeof(PassengerCarriege))]
    [XmlArrayItem("CargoCarriege", typeof(CargoCarriege))]
    public List<Carriege> carrieges = new();

    [XmlAttribute("Id")]
    public int Id {  get; set; }

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

    [XmlIgnore] 
    public Route route;






    public int Locomotives => carrieges.Count(
        item => item is Locomotive
        );

    public Train()
    {
        carrieges = new List<Carriege>();
    }

    public Train(TrainType type, List<Train> trains)
    {
        carrieges = new List<Carriege>();
        this.type = type;
        Id = IdGenerator.GetNextId(trains);
    }

    public Train(TrainType type, TrainCondition condition, string model, List<Train> trains)
    {
        carrieges = new List<Carriege>();
        this.type = type;
        this.condition = condition;
        this.model = model;
        Console.WriteLine(model);
        Id = IdGenerator.GetNextId(trains);
    }

    public void Add(Carriege carriege)
    {
        carrieges.Add(carriege);
    }

    public void Delete(int id)
    {
        carrieges.RemoveAt(id);
    }

    public void SetRoute(Route route)
    {
        //this.route = route;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append(model + " " + Id);

        foreach(Carriege car in carrieges)
        {
            sb.Append('\n' + car.ToString());
        }

        return sb.ToString();
    }
}