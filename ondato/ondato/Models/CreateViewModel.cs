using System;
using System.Collections.Generic;

namespace ondato.DTO
{
    public class CreateViewModel
    {
        public string Key { get; set; }

        public List<Object> Data { get; set; }

        public DateTime? Valid { get; set; }
    }
}      