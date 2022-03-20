namespace RawBot.Runtime.Logic.Skills
{
    public interface ISkillProvider
    {
        bool ShouldUseSkill(Context context);

        int GetNextSkillIndex(Context context);

        void OnTargetReset(Context context);

        void OnStop(Context context);
    }
}
