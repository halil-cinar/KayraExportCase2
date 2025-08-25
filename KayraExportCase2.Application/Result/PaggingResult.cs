using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportCase2.Application.Result
{
    public class PaggingResult<T> : SystemResult<T>
    {
        public int CurrentPage { get; set; }
        public int ItemCount { get; set; }
        public int AllCount { get; set; }
        public int AllPageCount { get; set; }
        public new List<T>? Data { get; set; }


       

    }
}
