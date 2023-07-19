using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProductNews.Helpers
{
    public class Utility
    {
        public static string SaveImage(IFormFile image)
        {
            string filePath = Path.Combine("./wwwroot/images/", image.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                image.CopyTo(fileStream);
            }

            return image.FileName;
        }

        //public static T GetObjectFromSession<T>(string name, HttpContext httpContext)
        //{
        //    string obj = httpContext.Session.GetString(name);
        //    if (obj != null)
        //    {
        //        Object o = JsonConvert.DeserializeObject<T>(obj);
        //        JObject jObj = obj is JObject ? (JObject)obj : JObject.FromObject(obj);
        //        return o.to;
        //    }
        //    return null;
        //}
    }
}
