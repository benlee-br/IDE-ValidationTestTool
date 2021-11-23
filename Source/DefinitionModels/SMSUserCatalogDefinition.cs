using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp.DefinitionModels
{
    public class SMSUserCatalogDefinition : DefinitionModelBase
    {
        protected override ImmutableList<DefinitionMigrationWarning> Migrate(int sourceVersion, ImmutableDictionary<string, object> serializedData)
        {
            throw new NotImplementedException();
        }
    }
}
