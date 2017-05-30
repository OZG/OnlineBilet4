using System.Web;
using System.Web.Mvc;

namespace Bonomini_Guido_TPF
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}