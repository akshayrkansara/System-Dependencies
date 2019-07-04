//Akshay, Tyler, Anil, Shubham
using System.Collections.Generic;

namespace System_Dependencies
{
    class Program
    {
        public string Name { get; set; }
        public List<Program> parent = new List<Program>();
        public List<Program> child = new List<Program>();
        public bool Installed { get; set; }      
    }
}
