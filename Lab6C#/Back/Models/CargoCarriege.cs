using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

[XmlRoot("CargoCarriege")]
public class CargoCarriege : Carriege
{
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

    public override string GetCarSpecType()
    {
        return type.ToString();
    }

    public override TrainType GetCarType()
    {
        return TrainType.Cargo;
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