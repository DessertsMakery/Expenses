using Ardalis.SmartEnum;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DessertsMakery.Common.Utility.BsonSerializers.SmartEnumSerializers;

internal sealed class SmartEnumSerializer<TEnum, TValue> : ClassSerializerBase<TEnum?>
    where TEnum : SmartEnum<TEnum, TValue>
    where TValue : IEquatable<TValue>, IComparable<TValue>
{
    private const string Value = nameof(Value);

    private static readonly BsonClassMap ClassMap = BsonClassMap.LookupClassMap(typeof(TEnum));
    private static readonly BsonClassMapSerializer<TEnum> BsonClassMapSerializer = new(ClassMap);

    protected override void SerializeValue(BsonSerializationContext context, BsonSerializationArgs args, TEnum? value)
    {
        if (value == null)
        {
            context.Writer.WriteNull();
            return;
        }

        BsonClassMapSerializer.Serialize(context, args, value);
    }

    protected override TEnum? DeserializeValue(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonReader = context.Reader;
        if (bsonReader.GetCurrentBsonType() == BsonType.Null)
        {
            bsonReader.ReadNull();
            return default;
        }

        bsonReader.ReadStartDocument();
        if (!bsonReader.FindElement(Value))
        {
            throw new InvalidOperationException($"SmartEnum should always define property with name: `{Value}`");
        }

        var member = ClassMap.AllMemberMaps.First(x => x.ElementName == Value);
        var value = member.GetSerializer().Deserialize(context);
        bsonReader.ReadEndDocument();

        return SmartEnum<TEnum, TValue>.FromValue((TValue)value);
    }
}
