namespace CartService
{
    public class Config
    {
        public string ProductServiceAddress { get; set; }
        public string PathToGetProduct { get; set; }
        
        public Config(ConfigurationManager config)
        {
            ProductServiceAddress = config["Services:ProductService"] ?? throw new NullReferenceException("Services:ProductService field is empty");
            PathToGetProduct = config["Paths:GetProductPath"] ?? throw new NullReferenceException("Paths:GetProductPath is empty");
        }
    }
}
