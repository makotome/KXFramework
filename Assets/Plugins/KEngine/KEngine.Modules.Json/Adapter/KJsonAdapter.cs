using UnityEngine;
using System.IO;
using System.Collections.Generic;

// [TIPS] : JsonDotNet download URL : http://www.parentelement.com/assets/json_net_unity
// [TIPS] : SimpleJson download URL : https://github.com/facebook-csharp-sdk/simple-json/tree/master/src/SimpleJson

namespace KEngine.Lib
{
    public class KJsonAdapter// : MonoBehaviour
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = "";
#if JSON_DOT_NET
            json = Newtonsoft.Json.JsonConvert.SerializeObject(o);
#elif SIMPLE_JSON
            return SimpleJson.SimpleJson.SerializeObject(item);
#else
            Log.Error("[TIPS]: SimpleJson or JsonDotNet, Please add a macro definition SIMPLE_JSON or JSON_DOT_NET.");
            return json;
#endif
        }

        public static T DeserializeObject<T>(string json)
        {
#if JSON_DOT_NET
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
#elif SIMPLE_JSON
            return SimpleJson.SimpleJson.DeserializeObject<T>(json);
#else
            Log.Error("[TIPS]: SimpleJson or JsonDotNet, Please add a macro definition SIMPLE_JSON or JSON_DOT_NET.");
            return default(T);
#endif
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
#if JSON_DOT_NET
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new Newtonsoft.Json.JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
#elif SIMPLE_JSON
            Log.Error("[TIPS]: The SimpleJson library cannot provide this method. Please use the JsonDotNet library.");
#else
            Log.Error("[TIPS]: SimpleJson or JsonDotNet, Please add a macro definition SIMPLE_JSON or JSON_DOT_NET.");
            return default(T);
#endif
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
#if JSON_DOT_NET
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new Newtonsoft.Json.JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
#elif SIMPLE_JSON
            Log.Error("[TIPS]: The SimpleJson library cannot provide this method. Please use the JsonDotNet library.");
#else
            Log.Error("[TIPS]: SimpleJson or JsonDotNet, Please add a macro definition SIMPLE_JSON or JSON_DOT_NET.");
            return null;
#endif
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
#if JSON_DOT_NET
            return Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
#elif SIMPLE_JSON
            Log.Error("[TIPS]: The SimpleJson library cannot provide this method. Please use the JsonDotNet library.");
#else
            Log.Error("[TIPS]: SimpleJson or JsonDotNet, Please add a macro definition SIMPLE_JSON or JSON_DOT_NET.");
            return default(T);
#endif
        }
    }
}