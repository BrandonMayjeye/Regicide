namespace Mayjeye.Extensions
{
    public static class Extensions
    {
        public static string ReadJson(this string s) => System.IO.File.ReadAllText(s);
        public static T ParseJson<T>(this string s) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s.ReadJson());
        public static string ToJson(this object o, bool format = false) =>
        Newtonsoft.Json.JsonConvert.SerializeObject(o, format ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None);

    }
}