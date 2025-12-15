using System.Xml.Serialization;

[XmlInclude(typeof(Locomotive))]
[XmlInclude(typeof(PassengerCarriege))]
[XmlInclude(typeof(CargoCarriege))]
public abstract class Carriege
{
    [XmlAttribute]
    public int carryingCapacity {  get; set; }

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
    public virtual void Add(Item item)
    {

    }
    public virtual string GetType()
    {
        return "BaseType";
    }

    public virtual double GetTypeCostModifier()
    {
        return -1.0;
    }
}