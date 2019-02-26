using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iv.Common
{
    public class OutputModel<T>
    {
        public bool Result { get; set; } = true;
        public T Data { get; set; }
        public string Message { get; set; }

        public OutputModel()
        {

        }

        public OutputModel(T data) : this()
        {
            this.Data = data;
        }

    }
}