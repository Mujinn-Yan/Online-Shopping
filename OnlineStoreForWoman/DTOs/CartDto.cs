namespace OnlineStoreForWoman.DTOs
{
    public class CartDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Bill { get; set; }
        public string? PicturePath { get; set; }
    }
}
