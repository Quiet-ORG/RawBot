namespace RawBot.Runtime.Logic.Skills
{
    public interface IRule
    {
        bool ShouldUse(Context context);
    }

    public class AllowRule : IRule
    {
        public bool ShouldUse(Context context) => true;
    }
}