using System;
using System.Collections.Generic;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Jayrock.Json.Conversion.Converters;
namespace Com.Zfrong.CommonLib.Jayrock
{
   

    public class GenericListImporter<T> : ImporterBase
    {
        public GenericListImporter() : base(typeof(IList<T>)) { ; }

        protected override object ImportFromArray(ImportContext context, JsonReader reader)
        {
            List<T> list = new List<T>(); 
            Type elementType = typeof(T);
            reader.Read();
            while (reader.TokenClass != JsonTokenClass.EndArray) 
                list.Add((T)context.Import(elementType, reader));
            reader.Read(); 
            return list.ToArray();
        }
    } 
}