using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp.DefinitionModels
{
    public enum DefinitionMigrationWarningType
    {
        TargetVersionIsNewer,
        TargetVersionIsOlder,
        MissingField,
        InvalidValue,
    }


    public class DefinitionMigrationWarning
    {
        public DefinitionMigrationWarning(
            Type definitionType,
            DefinitionMigrationWarningType warningType,
            int sourceVersion,
            int targetVersion,
            string details)
        {
            DefinitionType = definitionType;
            WarningType = warningType;
            SourceVersion = sourceVersion;
            TargetVersion = targetVersion;
            Details = details;
        }

        public Type DefinitionType { get; set; }
        public int SourceVersion { get; set; }
        public int TargetVersion { get; set; }
        public DefinitionMigrationWarningType WarningType { get; set; }
        public string Details { get; set; }
    }
}
