using NewNew.Models.Domain;

namespace NewNew.Models
{
    public class UpdatePublisherViewModel
    {
        public int PublisherId { get; set; }
        public string publisherName { get; set; }
        public string publisherAddress { get; set; }
        public ICollection<BookCopy> copies { get; set; }
    }
}
