public class CargoCarriege : Carriege
{
    List<Cargo> cargos = new();

    public CargoCarriege() { }

    public CargoCarriege(int carryingCapacity) : base(carryingCapacity) { }

    public override string GetClass()
    {
        return "Cargo Carriege\t";
    }
}