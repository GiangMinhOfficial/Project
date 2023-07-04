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
    }
}
