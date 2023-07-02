using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StockPulse.Helpers
{
    public class BusinessSettingsLoader
    {


        public static BusinessSettings Load()
        {
            string json = File.ReadAllText("Properties/BusinessSettings.json");
            if (json == null)
            {
                return new();
            }

            return JsonSerializer.Deserialize<BusinessSettings>(json);
        }

        public static void Save(BusinessSettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            if (json == null)
            {
                return;
            }

            File.WriteAllText("Properties/BusinessSettings.json", json);
        }
    }

    public class BusinessSettings
    {

        [Display(Name = "Default employee password")]
        [Required]
        public string DefaultEmployeePassword { get; set; }

        [Display(Name = "Default monthly salary in euros")]
        [Required]
        public double DefaultEmployeeSalary { get; set; }
    }
}
