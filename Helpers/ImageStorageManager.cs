using StockPulse.Interfaces;

namespace StockPulse.Helpers
{
    public class ImageStorageManager
    {
        public static string UploadImage(ICanContainImage model, IWebHostEnvironment env)
        {
            if (model.FormFile == null)
            {
                model.SetImagePath("notfound.png");
                return null;
            }

            // Get the name of the class, and thus the folder name.
            Type type = model.GetType();
            string class_name = type.Name.ToLower();

            // Make the complete file name/
            string upload_folder = Path.Combine(env.WebRootPath, $"lib/uploads/{class_name}");
            string unique_file_name = Guid.NewGuid().ToString() + "_" + model.FormFile.FileName;
            string filepath = Path.Combine(upload_folder, unique_file_name);

            // Save the file.
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                model.FormFile.CopyTo(stream);
                model.SetImagePath(unique_file_name);
            }

            return unique_file_name;
        }

        public static void RemoveImage(ICanContainImage model, IWebHostEnvironment env)
        {
            if (model.GetImagePath() == null || model.GetImagePath().Equals("notfound.png"))
            {
                return;
            }

            // Get the name of the class, and thus the folder name.
            Type type = model.GetType();
            string class_name = type.Name.ToLower();

            // Make the complete file name/
            string folder = Path.Combine(env.WebRootPath, $"lib/uploads/{class_name}");
            string fp = Path.Combine(folder, model.GetImagePath());

            // Delete the image     
            if (File.Exists(fp))
            {
                File.Delete(fp);
            }

        }
    }
}
