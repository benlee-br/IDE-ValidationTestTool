using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using WpfTestApp.DefinitionModels;
using System.Threading;

namespace WpfTestApp.ViewModelBase
{
    public abstract class ViewModelBase<T> : ObservableObject
        where T : DefinitionModelBase, new()
    {
        JObject property;

        DefinitionModelBase _Definition;
        protected DefinitionModelBase Definition
        {
            get { return _Definition; }
            set { _Definition = value; }
        }
        public ViewModelBase()
        {

        }

        void InitializeDefinition(T definition)
        {

            Definition = definition;
        }

        public static string Serialize(T definition)
        {
            return JsonConvert.SerializeObject(
                definition,
                Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize });
        }

        public static T Deserialize(string definitionString)
        {

            var properties = JsonConvert.DeserializeObject<ImmutableDictionary<string, object>> (definitionString);

            var item = new T();
            var migrationWarnings = item.Deserialize(properties);
            return item;
        }
        public void Deserialize()
        {
            var properties = GetType().GetRuntimeProperties().OrderBy((p) => p.Name);

            foreach (var propertyInfo in properties)
            {
                var item = propertyInfo.GetValue(this);
                if (item is DefinitionModelBase)
                {
                    var childDefinitionModel = (DefinitionModelBase)item;

                }
            }
                //                    else if (propertyInfo.PropertyType.Name == "DefinitionModelCollection`1")
                //                    {
                //                        var definitionsProperty = item.GetType().GetRuntimeProperty("DefinitionModels");
                //var definitions = (IEnumerable)definitionsProperty.GetValue(item);
                //                        foreach (DefinitionModelBase definition in definitions)
                //                        {
                //                            messageComponents += definition.DigestCalculate(notificationClient, notifierModel);
                //                        }

                //                    }
                //                }
                //            }
        }

    }
}
