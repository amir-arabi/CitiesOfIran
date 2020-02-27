using System;
using System.Collections.Generic;
using System.Text;

namespace CitiesOfIran
{
    class Model
    {
        public string State { get; set; }
        public List<string> Cities { get; set; }
        
        public Model(string state, List<string> cities)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            Cities = cities ?? throw new ArgumentNullException(nameof(cities));
        }
    }
}
