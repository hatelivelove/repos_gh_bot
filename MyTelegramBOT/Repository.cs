using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyTelegramBOT
{
    public class Repository
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public virtual User User { get; set; }
    }
}
