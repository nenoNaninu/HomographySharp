using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using HomographySharp.Single;
using HomographySharp.Double;


namespace HomographySharp
{
    public class HomographyJsonConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(HomographyMatrix<float>) || typeToConvert == typeof(HomographyMatrix<double>);
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert == typeof(HomographyMatrix<float>))
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
                            var elements = JsonSerializer.Deserialize<float[]>(ref reader);

                            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
                            {
                                throw new JsonException("Json structure is not match.");
                            }

                            return new SingleHomographyMatrix(elements);
                        }
                    }
                }
            }

            if (typeToConvert == typeof(HomographyMatrix<double>))
            {
                if (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        var property = reader.GetString();

                        if (!string.Equals(property, "Elements", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new JsonException("A property named Elements is required to deserialize to HomographyMatrix<double>.");
                        }

                        if (reader.Read())
                        {
                            var elements = JsonSerializer.Deserialize<double[]>(ref reader);

                            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
                            {
                                throw new JsonException("Json structure is not match.");
                            }

                            return new DoubleHomographyMatrix(elements);
                        }
                    }
                }
            }

            throw new JsonException("Json structure is not match.");
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            if (value is HomographyMatrix<float> singleHomographyMatrix)
            {
                writer.WriteStartObject();
                var propertyName = options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase ? "elements" : "Elements";
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, singleHomographyMatrix.Elements);
                writer.WriteEndObject();
                return;
            }

            if (value is HomographyMatrix<double> doubleHomographyMatrix)
            {
                writer.WriteStartObject();
                var propertyName = options.PropertyNamingPolicy == JsonNamingPolicy.CamelCase ? "elements" : "Elements";
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, doubleHomographyMatrix.Elements);
                writer.WriteEndObject();
                return;
            }
        }
    }
}