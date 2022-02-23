using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesEF.Data
{
    public class Image
    {
        public string Title { get; set; }
        public DateTime DateUploaded { get; set; }   
        public int Id { get; set; }
        public int Likes { get; set; }
        public string FileName { get; set; }
    }
}
