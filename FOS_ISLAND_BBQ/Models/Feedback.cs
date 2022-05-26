using System;
using System.ComponentModel.DataAnnotations;

namespace FOS_ISLAND_BBQ.Models
{
    public class Feedback
    {
        //mention the structure of your content
        public string id { get; set; }
        [Display(Name = "Please enter your name")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Please enter your email")]
        [Required]
        public string email { get; set; }

        [Display(Name = "Please enter the feedback")]
        [Required]
        public string feedback { get; set; }

    }
}