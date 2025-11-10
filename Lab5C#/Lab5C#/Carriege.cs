public abstract class Carriege
{
    int carryingCapacity {  get; set; }

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
}