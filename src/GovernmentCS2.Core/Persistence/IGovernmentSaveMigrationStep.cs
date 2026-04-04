namespace GovernmentCS2.Core.Persistence
{
    public interface IGovernmentSaveMigrationStep
    {
        int FromVersion { get; }

        int ToVersion { get; }

        string Apply(string json);
    }
}
