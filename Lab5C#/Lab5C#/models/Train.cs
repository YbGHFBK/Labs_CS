using System.Text;
using System.Xml.Serialization;

[XmlRoot("Train")]
public class Train
{
    [XmlArray("Carrieges")]
    [XmlArrayItem("Locomotive", typeof(Locomotive))]
    [XmlArrayItem("PassengerCarriege", typeof(PassengerCarriege))]
    [XmlArrayItem("CargoCarriege", typeof(CargoCarriege))]
    public List<Carriege> carrieges = new();

    [XmlAttribute("Id")]
    public int id;

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

    public Train(TrainType type)
    {
        carrieges = new List<Carriege>();
        this.type = type;
    }

    public Train(TrainType type, TrainCondition condition)
    {
        carrieges = new List<Carriege>();
        this.type = type;
        this.condition = condition;
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

        sb.Append(model + " " + id);

        foreach(Carriege car in carrieges)
        {
            sb.Append(car.ToString() + '\n');
        }

        return sb.ToString();
    }
}