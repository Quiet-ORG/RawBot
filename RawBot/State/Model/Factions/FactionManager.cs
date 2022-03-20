namespace RawBot.State.Model.Factions
{
    public class FactionManager : ListManager<Faction, int>
    {
        protected override int GetKey(Faction faction)
        {
            return faction.Id;
        }
    }
}