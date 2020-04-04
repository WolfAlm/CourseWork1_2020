using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourseWork_Library
{
    class User
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public Company Company { get; set; }
        public List<string> Languages { get; set; }
    }
}
