using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Jayrock.Json.Conversion.Converters;
namespace Com.Zfrong.CommonLib.JayRock
{
    public class GenericImporter<T> : IImporter
{
    public Type OutputType
    {
        get { return typeof(T); }
    }

    public object Import(ImportContext context, JsonReader reader)
    {
        if (context == null) throw new ArgumentNullException("context");
        if (reader == null) throw new ArgumentNullException("reader");

        if (!reader.MoveToContent())
            throw new JsonException("Unexpected EOF.");

        if (reader.TokenClass == JsonTokenClass.Null)
        {
            reader.Read();
            return null;
        }

        return context.Import(typeof(T), reader);
    } 
    }
}
