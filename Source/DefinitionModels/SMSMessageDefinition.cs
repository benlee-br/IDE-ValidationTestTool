using System;
using System.Collections.Immutable;


namespace WpfTestApp.DefinitionModels
{
    public class SMSMessageDefinition : DefinitionModelBase
    {
        public DateTime DateTime { get; set; }
        public MessageType MessageType { get; set; }
        public string Content { get; set; }
        public MessageAct Act { get; set; }
        private int Version = 1;
        protected override ImmutableList<DefinitionMigrationWarning> Migrate(int sourceVersion, ImmutableDictionary<string, object> serializedData)
        {
            var migrationWarnings = ImmutableList<DefinitionMigrationWarning>.Empty;
            if (serializedData.ContainsKey("Id") && serializedData["Id"] != null) { Id = serializedData["Id"].ToString(); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "Id")); }

            if (serializedData.ContainsKey("Content") && serializedData["Content"] != null) { Content = serializedData["Content"].ToString(); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "Content")); }
            if (serializedData.ContainsKey("MessageType") && serializedData["MessageType"] != null) { MessageType = ParseEnum<MessageType>(serializedData["MessageType"].ToString()); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "MessageType")); }
            if (serializedData.ContainsKey("DateTime") && serializedData["DateTime"] != null) { DateTime = DateTime.Parse( serializedData["DateTime"].ToString()); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "DateTime")); }
            if (serializedData.ContainsKey("Act") && serializedData["Act"] != null) { Act = ParseEnum<MessageAct>(serializedData["Act"].ToString()); }
            else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "Act")); }
            return migrationWarnings;
        }
        public static SMSMessageDefinition Build(
            string id,
            string content,
            MessageType messageType,
            DateTime ownerId,
            MessageAct act)
        {
            var sMessage = new SMSMessageDefinition()
            {
                Id = id,
                Content = content,
                MessageType = messageType,
                DateTime = ownerId,
                Act = act,
            };
            return sMessage;
        }


    }
    public enum MessageAct : int
    {
        Sent,
        Received
    }

    public enum MessageType : int
    {
        SMS = 0,
        MMS
    }

}