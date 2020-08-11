using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MyTelegramBOT
{
    public class User
    {
        public int Id { get; set; }
        public long ChatId { get; set; }

        public virtual List<Repository> Repositories{ get; set; }
    }
}
