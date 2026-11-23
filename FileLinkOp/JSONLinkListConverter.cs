using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLinkOp
{
    /// <summary>
    /// linkの要素をオブジェクトではなく文字列として解釈させる
    /// </summary>
    internal class JSONLinkListConverter : JsonConverter<List<string>>
    {
        /// <summary>
        /// 変換する処理
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="hasExistingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override List<string> ReadJson(JsonReader reader, Type objectType, List<string> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return array.Select(item => item.ToString(Formatting.None)).ToList();
        }

        /// <summary>
        /// 書き込む？
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, List<string> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var s in value)
            {
                // string を JSON として書き戻す
                writer.WriteRawValue(s);
            }
            writer.WriteEndArray();
        }
    }
}
