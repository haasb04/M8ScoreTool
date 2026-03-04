using System.Web;
using System.Web.Mvc;

namespace M8ScoreTool {
	public class FilterConfig {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new HandleErrorAttribute());
		}
	}
}
