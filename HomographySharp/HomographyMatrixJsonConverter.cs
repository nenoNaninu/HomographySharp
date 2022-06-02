using System;
using System.Collections.Generic;
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
        if (typeToConvert == typeof(HomographyMatrix<float>))
        {
            float[] elements = ReadElements<float>(ref reader);
            return new SingleHomographyMatrix(elements);
        }

        if (typeToConvert == typeof(HomographyMatrix<double>))
        {
            double[] elements = ReadElements<double>(ref reader);
            return new DoubleHomographyMatrix(elements);
        }

        throw new JsonException("Invalid JSON");
    }

    private static T[] ReadElements<T>(ref Utf8JsonReader reader)
    {
        if (!reader.Read())
        {
            throw new JsonException("Invalid JSON");
        }

        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            throw new JsonException("Invalid JSON");
        }

        var property = reader.GetString();

        if (!string.Equals(property, "Elements", StringComparison.OrdinalIgnoreCase))
        {
            throw new JsonException("A property named Elements is required to deserialize to HomographyMatrix<T>.");
        }

        if (!reader.Read())
        {
            throw new JsonException("Invalid JSON");
        }

        var elements = JsonSerializer.Deserialize<T[]>(ref reader);

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Invalid JSON");
        }

        if (elements is null)
        {
            throw new JsonException("HomographyMatrix<T>.Elements is null.");
        }

        return elements;
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value is HomographyMatrix<float> singleHomographyMatrix)
        {
            WriteCore(writer, options, singleHomographyMatrix.Elements);
            return;
        }

        if (value is HomographyMatrix<double> doubleHomographyMatrix)
        {
            WriteCore(writer, options, doubleHomographyMatrix.Elements);
            return;
        }

        throw new JsonException($"{value.GetType()} is unsupported type.");
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
