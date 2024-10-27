using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class P_ReturnMessageWithObj_Result : P_ReturnMessage_Result
    {
        public object? ID { get; set; } = null;
    }
    public class P_ReturnMessage_Result
    {
        public bool ReturnCode { get; set; } = false;
        public string? ReturnText { get; set; } = "";
    }
}
