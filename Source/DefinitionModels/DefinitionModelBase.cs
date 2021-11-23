using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp.DefinitionModels
{
    public abstract class DefinitionModelBase
    {
        public DefinitionModelBase() { }

        public DefinitionModelBase(string id, string name)
        {
            Id = id;
            Name = name;
        }


        public string Id { get; set; }
        public string Name { get; set; }

        private static readonly ImmutableHashSet<Type> DefaultDefinitionModelSupportedTypes =
            ImmutableHashSet<Type>.Empty
                .Add(typeof(bool))
                .Add(typeof(byte))
                .Add(typeof(char))
                .Add(typeof(DateTime))
                .Add(typeof(decimal))
                .Add(typeof(double))
                .Add(typeof(short))
                .Add(typeof(int))
                .Add(typeof(long))
                .Add(typeof(sbyte))
                .Add(typeof(float))
                .Add(typeof(string))
                .Add(typeof(TimeSpan))
                .Add(typeof(ushort))
                .Add(typeof(uint))
                .Add(typeof(ulong));
        private readonly ImmutableHashSet<Type> DefinitionModelSupportedTypes = DefaultDefinitionModelSupportedTypes;

        private int Version = 1;
        public ImmutableList<DefinitionMigrationWarning> Deserialize(object serializedObject)
        {
            var serialized = JsonConvert.SerializeObject(serializedObject);
            var properties = JsonConvert.DeserializeObject<ImmutableDictionary<string, object>>(serialized);
            return Deserialize(properties);
        }

        public ImmutableList<DefinitionMigrationWarning> Deserialize(ImmutableDictionary<string, object> serializedData)
        {
            var migrationWarnings = ImmutableList<DefinitionMigrationWarning>.Empty;


            var sourceVersion = 0;
            if (serializedData.ContainsKey("Version")) { sourceVersion = Convert.ToInt32(serializedData["Version"]); }
            {
                if (sourceVersion < Version)
                {
                    migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.TargetVersionIsNewer, sourceVersion, Version, "Version"));
                }
                else if (sourceVersion > Version)
                {
                    migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.TargetVersionIsOlder, sourceVersion, Version, "Version"));
                }
            }
            if (serializedData.ContainsKey("Id") && serializedData["Id"] != null) { Id = serializedData["Id"].ToString(); }
            //else { migrationWarnings = migrationWarnings.Add(new DefinitionMigrationWarning(GetType(), DefinitionMigrationWarningType.MissingField, sourceVersion, Version, "Id")); }

            if (serializedData.ContainsKey("Name") && serializedData["Name"] != null) { Name = serializedData["Name"].ToString(); }

            migrationWarnings = migrationWarnings.AddRange(Migrate(sourceVersion, serializedData));

            return migrationWarnings;
        }

        protected abstract ImmutableList<DefinitionMigrationWarning> Migrate(int sourceVersion, ImmutableDictionary<string, object> serializedData);

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }

    public abstract class DefinitionBase<T> : IDefinition<T>
       where T : DefinitionModelBase, new()
    {
        public DefinitionBase()
        {
        }


        public DefinitionBase(T definitionModel)
        {

            InitializeDefinition(definitionModel);
        }

        public T DefinitionModel { get; protected set; }

        public string Id { get { return DefinitionModel.Id; } }
        public string Name { get { return DefinitionModel.Name; } }

        public void UpdateDefinition(T definitionModel)
        {
            throw new NotImplementedException();
        }

        void InitializeDefinition(T definitionModel)
        {
            DefinitionModel = definitionModel ?? throw new DefinitionModelNullException(typeof(T));
        }

        public static string Serialize(T definitionModel)
        {
            return JsonConvert.SerializeObject(
                definitionModel,
                Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Serialize });
        }
    }

    public class DefinitionModelNullException : Exception
    {
        public DefinitionModelNullException(Type definitionModelType) : base(definitionModelType.FullName)
        {
        }
    }


    public interface IDefinition<T> where T : DefinitionModelBase
    {
        T DefinitionModel { get; }
        string Name { get; }
        void UpdateDefinition(T definitionModel);
    }
}
