using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GovernmentCS2.Core.Persistence
{
    public sealed class GovernmentSaveMigrationException : Exception
    {
        public GovernmentSaveMigrationException(string message)
            : base(message)
        {
        }
    }

    public sealed class GovernmentSaveMigrationRunner
    {
        private readonly IDictionary<int, IGovernmentSaveMigrationStep> mySteps;

        public GovernmentSaveMigrationRunner(IEnumerable<IGovernmentSaveMigrationStep> steps)
        {
            mySteps = (steps ?? Enumerable.Empty<IGovernmentSaveMigrationStep>())
                .ToDictionary(step => step.FromVersion);
        }

        public string MigrateToVersion(string json, int targetVersion)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new GovernmentSaveMigrationException("Government save payload cannot be empty.");
            }

            var currentVersion = ReadSchemaVersion(json);
            var migratedJson = json;

            while (currentVersion < targetVersion)
            {
                if (!mySteps.TryGetValue(currentVersion, out var step))
                {
                    throw new GovernmentSaveMigrationException($"No migration step is registered from schema version {currentVersion} to {targetVersion}.");
                }

                migratedJson = step.Apply(migratedJson);
                currentVersion = step.ToVersion;
            }

            if (currentVersion != targetVersion)
            {
                throw new GovernmentSaveMigrationException($"Government save payload ended on schema version {currentVersion}, expected {targetVersion}.");
            }

            return migratedJson;
        }

        public int ReadSchemaVersion(string json)
        {
            using (var document = JsonDocument.Parse(json))
            {
                if (document.RootElement.TryGetProperty("SchemaVersion", out var schemaVersionProperty))
                {
                    return schemaVersionProperty.GetInt32();
                }

                if (document.RootElement.TryGetProperty("schemaVersion", out schemaVersionProperty))
                {
                    return schemaVersionProperty.GetInt32();
                }
            }

            return 0;
        }
    }
}
