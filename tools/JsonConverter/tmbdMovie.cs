using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

public struct tmbdMovie
{
    [JsonConverter(typeof(GenresEnumConverter))]
    public List<Genre> genres;
    public string original_language;
    public string overview;
    [JsonConverter(typeof(DateOnlyConverter))]
    public DateOnly release_date;
    public int runtime;
    public string title;
    public float vote_average;
    [JsonConverter(typeof(CertificationConverter))]
    public Certification certification;
    [JsonConverter(typeof(DictionaryToListConverter))]
    public List<string> directors;
    [JsonConverter(typeof(DictionaryToListConverter))]
    public List<string> writers;
    [JsonConverter(typeof(DictionaryToListConverter))]
    public List<string> cast;
}

public class DictionaryToListConverter : JsonConverter<List<string>>
{
    public override void WriteJson(JsonWriter writer, List<string> value, JsonSerializer serializer)
    {
        JToken t = JToken.FromObject(value);
        t.WriteTo(writer);
    }

    public override List<string> ReadJson(JsonReader reader, Type objectType, List<string> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        List<string> result = new List<string>();

        if (token.Type == JTokenType.Array)
        {
            foreach (var item in token)
            {
                if (item.Type == JTokenType.Object)
                {
                    var obj = (JObject)item;
                    foreach (var property in obj.Properties())
                    {
                        if(property.Name == "name"){
                            result.Add(property.Value.ToString());
                        }
                    }
                }
                else if (item.Type == JTokenType.String)
                {
                    result.Add(item.ToString());
                }
            }
        }

        return result;
    }
}

public class DateOnlyConverter : JsonConverter<DateOnly>
{
    private const string DateFormat = "yyyy-MM-dd"; // or your preferred date format

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(DateFormat));
    }

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string dateStr = (string)reader.Value;
        return DateOnly.ParseExact(dateStr, DateFormat);
    }
}

public class GenresEnumConverter : JsonConverter<List<Genre>>
{
    public override void WriteJson(JsonWriter writer, List<Genre> value, JsonSerializer serializer)
    {
        JArray array = new JArray(value.Select(g => g.ToString()));
        array.WriteTo(writer);
    }

    public override List<Genre> ReadJson(JsonReader reader, Type objectType, List<Genre> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        List<Genre> genres = new List<Genre>();

        if (token.Type == JTokenType.Array)
        {
            foreach (var item in token)
            {
                if (Enum.TryParse(item.ToString(), true, out Genre genre))
                {
                    genres.Add(genre);
                }
            }
        }
        else if (token.Type == JTokenType.String)
        {
            if (Enum.TryParse(token.ToString(), true, out Genre genre))
            {
                genres.Add(genre);
            }
        }

        return genres;
    }
}

public class CertificationConverter : JsonConverter<Certification>
{
    public override void WriteJson(JsonWriter writer, Certification value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override Certification ReadJson(JsonReader reader, Type objectType, Certification existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string certStr = (string)reader.Value;

        // Normalize common variations
        certStr = certStr.Replace("-", "").ToUpper(); // e.g., "PG-13" -> "PG13"

        if (Enum.TryParse(certStr, true, out Certification certification))
        {
            return certification;
        }
        return Certification.NONE; // Default value if not recognized
    }
}