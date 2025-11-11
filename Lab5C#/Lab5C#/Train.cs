using System.Text;
using System.Xml.Serialization;

[XmlRoot("Train")]
public class Train
{
    [XmlElement("Locomotive", typeof(Locomotive))]
    [XmlElement("PassengerCarriege", typeof(PassengerCarriege))]
    [XmlElement("CargoCarriege", typeof(CargoCarriege))]
    public List<Carriege> carrieges = new();

    public int Locomotives => carrieges.Count(
        item => item is Locomotive
        );

    public Train()
    {
        carrieges = new List<Carriege>();
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

        foreach(Carriege car in carrieges)
        {
            sb.Append(car.ToString() + '\n');
        }

        return sb.ToString();
    }
}