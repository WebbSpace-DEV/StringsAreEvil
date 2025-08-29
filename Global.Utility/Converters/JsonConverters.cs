using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Global.Utility.Converters;

public static class JsonConverters
{
    public static string Beautify(string json)
    {
        return JValue.Parse(json).ToString(Formatting.Indented);
    }
}

public static class JsonConverters<TEntity> where TEntity : class
{
    public static string Serialize(TEntity model)
    {
        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.SerializeObject(model, settings);
    }

    public static TEntity Deserialize(string json)
    {
        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include
        };

        return JsonConvert.DeserializeObject<TEntity>(json, settings);
    }

    public static string ListSerialize(IList<TEntity> model)
    {
        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        };

        return JsonConvert.SerializeObject(model, settings);
    }

    public static IList<TEntity> ListDeserialize(string json)
    {
        var settings = new JsonSerializerSettings()
        {
            ContractResolver = new NullSetterContractResolver(),
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Include
        };

        return JsonConvert.DeserializeObject<IList<TEntity>>(json, settings);
    }
}

public class NullSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        if (!prop.Writable)
        {
            if (member is PropertyInfo propertyInfo)
            {
                if (propertyInfo.GetSetMethod() is null)
                {
                    prop.ShouldSerialize = i => false;
                    prop.ShouldDeserialize = i => false;
                    prop.Ignored = true;
                }
            }
        }

        return prop;
    }
}
