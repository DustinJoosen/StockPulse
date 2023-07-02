namespace StockPulse.Interfaces
{
    public interface ICanContainImage
    {
        public IFormFile? FormFile { get; set; }

        public string GetImagePath();
        public void SetImagePath(string imagePath);

    }
}
