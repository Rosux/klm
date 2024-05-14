using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
/// <summary>
/// This program overrides JsonConverter.ReadJson from Newtonsoft to accept 
/// both strings and list of strings, and turns them into  a list of strings
/// This is because our films.json stores movies with a single genre as string
/// but multiple genres as a list.
/// Linked via "JsonConverter(typeof(JsonConverter<string>))]" in film.cs
/// </summary>
public class MovieConverter<T> : JsonConverter
{
    public MovieConverter()
    {

    }
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(List<T>));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JToken token = JToken.Load(reader);
        if (token.Type == JTokenType.Array)
        {
            return token.ToObject<List<T>>();
        }
        if (token.Type == JTokenType.String)
        {
            var singleValue = token.ToObject<T>();
            return new List<T> { singleValue }; 
        }
        throw new JsonSerializationException("Unexpected token type: " + token.Type);
    }
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var list = (List<T>)value;

        if (list.Count == 1)
        {
            JToken.FromObject(list[0]).WriteTo(writer);
        }
        else
        {
            JArray.FromObject(list).WriteTo(writer);
        }
    }


}