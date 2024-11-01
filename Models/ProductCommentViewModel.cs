namespace dotNetShop.Models
{

    public class ProductCommentViewModel
    {
        public Product Product { get; set; }
        public Comment NewComment { get; set; } // Модель для нового комментария 
        public List<Comment> Comments { get; set; } // Список всех комментариев к продукту }
    }
}