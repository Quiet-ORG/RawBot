namespace RawBot.State.Model.Items
{
    public class Inventory<T> : ListManager<T, int> where T : ItemBase
    {
        protected override int GetKey(T item)
        {
            return item.Id;
        }
    }
}
