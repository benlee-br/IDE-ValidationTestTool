using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace WpfTestApp.DefinitionModels
{
    public class SMSDepositoryDefinition : DefinitionModelBase
    {
        public SMSDepositoryDefinition() 
        {
            Messages = ImmutableList<SMSMessageDefinition>.Empty;
        }
        public string Owner { get; set; }
        public string OwnerId { get; set; }

        private int Version = 1;
        public ImmutableList<SMSMessageDefinition> Messages { get; set; }
        protected override ImmutableList<DefinitionMigrationWarning> Migrate(int sourceVersion, ImmutableDictionary<string, object> serializedData)
        {
            var migrationWarnings = ImmutableList<DefinitionMigrationWarning>.Empty;
            if (serializedData.ContainsKey("OwnerId") && serializedData["OwnerId"] != null) { OwnerId = serializedData["OwnerId"].ToString(); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "OwnerId")); }
            if (serializedData.ContainsKey("Owner") && serializedData["Owner"] != null) { Owner = serializedData["Owner"].ToString(); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "Owner")); }

            if (serializedData.ContainsKey("Messages") && serializedData["Messages"] != null)
            {
                var serializedMessagess = (IEnumerable<object>)serializedData["Messages"];
                foreach (var serializedMessage in serializedMessagess)
                {
                    var message = new SMSMessageDefinition();
                    migrationWarnings = migrationWarnings.AddRange(message.Deserialize(serializedMessage));
                    Messages = Messages.Add(message);
                }
            }
            return migrationWarnings;
        }

        public static SMSDepositoryDefinition Build(
            ImmutableList<SMSMessageDefinition> messages,
            string owner,
            string ownerId,
            string id,
            string name)
        {
            var sMSDepository = new SMSDepositoryDefinition()
            {
                Messages = messages,
                Owner = owner,
                OwnerId = ownerId,
                Id = id,
                Name = name
            };
            return sMSDepository;
        }

    }
}
