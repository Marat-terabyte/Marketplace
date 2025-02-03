namespace CartService.Models.InputModels
{
    public class BuyInput
    {
        public SelectedCartProduct[] SelectedCartProducts { get; set; }
        public string DeliveryPlace { get; set; }
    }
}
