using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using HomographySharp.Double;
using HomographySharp.Single;

namespace HomographySharp;

public sealed class HomographyMatrixJsonConverter : JsonConverter<object>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(HomographyMatrix<float>) || typeToConvert == typeof(HomographyMatrix<double>);
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var genericTypeArgumentName = typeToConvert.GenericTypeArguments.FirstOrDefault()?.Name;

        return genericTypeArgumentName switch
        {
            "Single" => new SingleHomographyMatrix(ReadElements<float>(ref reader)),
            "Float" => new SingleHomographyMatrix(ReadElements<float>(ref reader)),
            "Double" => new DoubleHomographyMatrix(ReadElements<double>(ref reader)),
            _ => throw new JsonException($"{genericTypeArgumentName} is not a valid generic type argument.")
        };
    }

    private static T[] ReadElements<T>(ref Utf8JsonReader reader)
    {
        if (!reader.Read())
        {
            throw new JsonException("JSON structure is not correct.");
        }

        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("JSON structure is not correct.");
        }

        var property = reader.GetString();

        if (!string.Equals(property, "Elements", StringComparison.OrdinalIgnoreCase))
        {
            throw new JsonException("A property named Elements is required to deserialize to HomographyMatrix<T>.");
        }

        if (!reader.Read())
        {
            throw new JsonException("JSON structure is not correct.");
        }

        var elements = JsonSerializer.Deserialize<T[]>(ref reader);

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject || elements is null)
        {
            throw new JsonException("JSON structure is not correct.");
        }

        if (elements is null)
        {
            throw new JsonException("HomographyMatrix<T>.Elements is null.");
        }

        return elements;

    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case HomographyMatrix<float> singleHomographyMatrix:
                WriteCore(writer, options, singleHomographyMatrix.Elements);
                return;
            case HomographyMatrix<double> doubleHomographyMatrix:
                WriteCore(writer, options, doubleHomographyMatrix.Elements);
                return;
            default:
                throw new JsonException($"{value.GetType()} is unsupported type.");
        }
    }

    private static void WriteCore<T>(Utf8JsonWriter writer, JsonSerializerOptions options, IReadOnlyList<T> elements)
    {
        writer.WriteStartObject();
        var propertyName = options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase ? "elements" : "Elements";
        writer.WritePropertyName(propertyName);
        JsonSerializer.Serialize(writer, elements);
        writer.WriteEndObject();
    }
}
