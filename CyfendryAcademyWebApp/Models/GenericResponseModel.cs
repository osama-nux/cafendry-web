using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.Models
{
    public class GenericResponseModel<T>
    {
        public bool status { get; set; }
        public string successMessage { get; set; }
        public string errorMessage { get; set; }
        public string userMessage { get; set; }
        public T model { get; set; }
        public IEnumerable<object> objModelList { get; set; }
        public IEnumerable<T> modelList { get; set; }
    }
}