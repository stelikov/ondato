using System;
using System.Collections.Generic;

namespace ondato.Models { 
    public class DictionaryViewModel
    {
        public string Key { get; set; }

        public List<Object> Data { get; set; }

        public DateTime? Expire { get; set; }
    }
}