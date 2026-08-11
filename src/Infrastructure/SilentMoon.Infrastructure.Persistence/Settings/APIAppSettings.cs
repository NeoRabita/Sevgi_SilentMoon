
namespace SilentMoon.Infrastructure.Persistence.Settings
{
    public class APIAppSettings
    {
        public string ConnectionString { get; set; }
        public string ClientAppOrigin { get; set; }
        public JWTSettings JWTSettings { get; set; }
        public MinioSettings MinioSettings { get; set; }
    }
}