using System.Xml.Serialization;

[XmlInclude(typeof(Locomotive))]
[XmlInclude(typeof(PassengerCarriege))]
[XmlInclude(typeof(CargoCarriege))]
public abstract class Carriege
{
    [XmlAttribute]
    public int carryingCapacity { get; set; }

    [XmlAttribute("CarType")]
    public TrainType carType;




    public Carriege() { }

    public Carriege(int carriyngCapacity)
    {
        this.carryingCapacity = carriyngCapacity;
    }

    public override string ToString()
    {
        return "Type: " + GetClass() + " \t| carrying capacity: " + carryingCapacity;
    }

    public virtual string GetClass()
    {
        return "Carriege";
    }
    public virtual string GetCarSpecType()
    {
        return "BaseType";
    }

    public abstract TrainType GetCarType();

    public virtual double GetTypeCostModifier()
    {
        return -1.0;
    }
}