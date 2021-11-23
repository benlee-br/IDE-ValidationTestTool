using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp.DefinitionModels
{
    public class FcsHeaderdefinition : DefinitionModelBase
    {
        private Dictionary<string, Dictionary<string, string>> _customlabelMapDefinition = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<string, Dictionary<string, string>> CustomlabelMapDefinition { get; set; }

        protected override ImmutableList<DefinitionMigrationWarning> Migrate(int sourceVersion, ImmutableDictionary<string, object> serializedData)
        {
            var migrationWarnings = ImmutableList<DefinitionMigrationWarning>.Empty;
            if (serializedData.ContainsKey("CustomlabelMapDefinition") && serializedData["CustomlabelMapDefinition"] != null)
            {
                string data = serializedData["CustomlabelMapDefinition"].ToString();
                CustomlabelMapDefinition = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(data);

            }

            return migrationWarnings;

        }
    }
}
