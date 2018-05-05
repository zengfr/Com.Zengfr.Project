using System.Collections.Generic;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using Jayrock.Json.Conversion.Converters;
namespace Com.Zfrong.CommonLib.Jayrock
{
    

    public class ListImporter<T> : ImporterBase where T:class
    {
        public ListImporter() :
            base(typeof(IList<T>)) { }

        protected override object ImportFromArray(ImportContext context, JsonReader reader)
        {
            var list = new List<T>();
            reader.Read(/* start */);
            while (reader.TokenClass != JsonTokenClass.EndArray)
            {
                 list.Add( context.Import(typeof(T), reader) as T);
            }
            reader.Read(/* end */);
            return list;
        }
    }
}