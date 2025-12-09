using System.Xml.Serialization;

[XmlRoot("CargoCarriege")]
public class CargoCarriege : Carriege
{
    [XmlElement("Cargo", typeof(Cargo))]
    public List<Cargo> cargos = new();

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
}