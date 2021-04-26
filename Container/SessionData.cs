using ShoppingCartCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartCA.Container
{
    public class SessionData
    {
        public string UserName { get; set; }
        public string SessionID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
