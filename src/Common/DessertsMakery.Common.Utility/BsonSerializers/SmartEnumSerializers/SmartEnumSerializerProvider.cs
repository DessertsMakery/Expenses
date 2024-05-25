using Ardalis.SmartEnum;
using MongoDB.Bson.Serialization;

namespace DessertsMakery.Common.Utility.BsonSerializers.SmartEnumSerializers;

internal sealed class SmartEnumSerializerProvider : BsonSerializationProviderBase
{
    private static readonly Type SmartEnumMarker = typeof(ISmartEnum);
    private static readonly Type OpenGenericSmartEnumType = typeof(SmartEnum<,>);
    private static readonly Type OpenGenericSerializerType = typeof(SmartEnumSerializer<,>);

    public override IBsonSerializer? GetSerializer(Type type, IBsonSerializerRegistry serializerRegistry)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!SmartEnumMarker.IsAssignableFrom(type))
        {
            return null;
        }

        var typeIsSmartEnum = IsAssignableToGenericType(type, OpenGenericSmartEnumType, out var arguments);
        if (typeIsSmartEnum)
        {
            return CreateGenericSerializer(OpenGenericSerializerType, arguments, serializerRegistry);
        }

        throw new InvalidOperationException($"Not found serializer for smart enum serialization for type: `{type}`");
    }

    private static bool IsAssignableToGenericType(Type givenType, Type genericType, out Type[] arguments)
    {
        while (true)
        {
            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                arguments = givenType.GetGenericArguments();
                return true;
            }

            var baseType = givenType.BaseType;
            if (baseType != null)
            {
                givenType = baseType;
                continue;
            }

            arguments = Array.Empty<Type>();
            return false;
        }
    }
}
