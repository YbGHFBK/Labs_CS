using System.Xml.Serialization;

[XmlRoot("CargoCarriege")]
public class CargoCarriege : Carriege
{
    [XmlElement("Cargo", typeof(Cargo))]
    public List<Cargo> cargos = new();
    [XmlAttribute("Type")]
    public CargoCarriegeType type;
    public CargoCarriege() { }

    public CargoCarriege(int carryingCapacity) : base(carryingCapacity) { }

    public override string GetClass()
    {
        return "Cargo Carriege\t";
    }

    public override string ToString()
    {
        return base.ToString() + " тонн";
    }

    public override void Add(Item item)
    {
        if (item is not Cargo) throw new FormatException("В грузовом вагоне могут находиться только грузы");

        cargos.Add((Cargo)item);
    }

    public override string GetType()
    {
        return type.ToString();
    }

    public override double GetTypeCostModifier()
    {
        switch (type)
        {
            case CargoCarriegeType.Platform:
                return 1.20;
            case CargoCarriegeType.Refrigerator:
                return 2.0;
            case CargoCarriegeType.OpenTop:
                return 1.40;
            case CargoCarriegeType.Covered:
                return 1.65;
        }
        return -1.0;
    }
}