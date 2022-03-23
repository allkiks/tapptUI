using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TAPPT.Web.Models
{
    public class PostSelectedViewModel
    {
        public int BounquetId { get; set; }
        public ICollection<int> SelectedContent { get; set; }
    }
}
