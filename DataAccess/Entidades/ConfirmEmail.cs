using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entidades
{
    public class ConfirmEmail
    {
        public int Id { get; set; }
        public int Usuario_Id { get; set; }
        public string Token { get; set; }
      
        public virtual Usuario Usuario { get; set; }
    }
}
