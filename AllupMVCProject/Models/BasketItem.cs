namespace AllupMVCProject.Models
{
    public class BasketItem:BaseEntity
    {
        public int ProductId { get; set; }
        public string AppUserId { get; set; }
        public int Count { get; set; }
        public AppUser appUser { get; set; }
        public Product product { get; set; }
    }
}
