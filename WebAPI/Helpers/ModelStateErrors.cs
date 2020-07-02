using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace WebAPI.Helpers
{
    public class ModelStateErrors
    {
        public static string DisplayModelStateError(ModelStateDictionary ModelState)
        {
            return ModelState.Values.Where(x => x.Errors.Count > 0).FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;         
        }
    }
}