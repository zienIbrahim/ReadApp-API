using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace APIAngular.Models
{

    public class User
    {

        public int Id { get; set; }

        public string Username { get; set; }

        public byte[] PassHash { get; set; }

        public byte[] SaltePass { get; set; }


    }



}
