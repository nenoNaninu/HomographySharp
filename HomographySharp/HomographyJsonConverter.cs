using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HomographySharp.Single;
using HomographySharp.Double;

namespace HomographySharp
{
    public sealed class HomographyJsonConverter : JsonConverter<object>
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

            throw new JsonException("JSON structure is not correct.");
        }

        private static T[] ReadElements<T>(ref Utf8JsonReader reader)
        {
            if (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var property = reader.GetString();

                    if (!string.Equals(property, "Elements", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new JsonException("A property named Elements is required to deserialize to HomographyMatrix<float>.");
                    }

                    if (reader.Read())
                    {
                        var elements = JsonSerializer.Deserialize<T[]>(ref reader);

                        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject || elements is null)
                        {
                            throw new JsonException("JSON structure is not correct.");
                        }

                        return elements;
                    }
                }
            }

            throw new JsonException("JSON structure is not correct.");
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
}