using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BradleyAquatics.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        {
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        public string Body { get; set; }
        public Nullable<int> Sorting { get; set; }
        public Nullable<bool> HasSideBar { get; set; }

    }
}