using Repository.Entities.BaseModelEntity;

namespace Repository.Entities
{
    public class QuizGroupItem : BaseEntity
    {
        public string? AudioUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? AudioScript { get; set; }
        public string? ImageDescription { get; set; }
        public string? PassageText { get; set; }
    }
}
