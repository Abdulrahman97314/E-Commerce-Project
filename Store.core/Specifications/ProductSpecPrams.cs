namespace Store.APIs.Helpers
{
    public class ProductSpecPrams
    {
        private const int MAXPAGESIZE = 20;
        private int pageSize = 6;
        public int PageSize 
        {
            get { return pageSize; } 
            set { pageSize = value > MAXPAGESIZE ? MAXPAGESIZE : value; }
        }
        public int PageIndex { get; set; } = 1;
        public string? Sort {get; set;}
        public int? BrandId { get; set;}
        public int? TypeId { get; set;}
        public string? Search { get; set;}
    }
}
