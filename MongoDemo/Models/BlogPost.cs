using System.Collections.Generic;

namespace MongoDemo.Models
{
    public class BlogPost : MongoModelBase<BlogPost>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
