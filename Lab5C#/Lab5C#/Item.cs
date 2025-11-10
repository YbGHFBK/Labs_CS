public abstract class Item
{
    int weight {  get; set; }

    public Item() { }

    public Item(int weight)
    {
        this.weight = weight;
    }
}