using System.Collections.Generic;

namespace Packt.CS7
{// THis is an "extra" model - in the main NorthwindMVS project. (Besides the ones in the library.)
    public class HomeIndexViewModel
    {
        public int VisitorCount;
        public IList<Category> Categories { get; set; }
        public IList<Product> Products { get; set; }
    }
}