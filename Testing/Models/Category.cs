using System.Collections;

namespace Testing.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public IEnumerable Categories { get; set; }

    }
}
