using System.Web;
using System.Web.Optimization;

namespace Com.Zengfr.CodeGenerator.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/js").Include());
            bundles.Add(new StyleBundle("~/Content/css").Include());

            bundles.Add(new ScriptBundle("~/Content/extjs").Include(
                "~/Content/extjs/ext-all-debug.js"
                ).Include(
                "~/Content/extjs/locale-zh_CN.js"
                )
                );
            bundles.Add(new StyleBundle("~/Content/extjs/theme-classic").Include(
                "~/Content/extjs/theme-classic/resources/theme-classic-all.css"
                ));

            bundles.Add(new ScriptBundle("~/App/js")
                .Include(
             "~/Appjs/Company_dataModel.js", "~/Appjs/Company_dataStore.js", 
             "~/Appjs/Company_formPanel.js", "~/Appjs/Company_infoPanel.js",  "~/Appjs/Company_searchPanel.js",
             "~/Appjs/Company_gridPanel.js", "~/Appjs/Company_list.js",  "~/Appjs/Company_propertyGrid.js"
              )

              .Include(
             "~/Appjs/Database_dataModel.js", "~/Appjs/Database_dataStore.js",
             "~/Appjs/Database_formPanel.js", "~/Appjs/Database_infoPanel.js", "~/Appjs/Database_searchPanel.js",
             "~/Appjs/Database_gridPanel.js", "~/Appjs/Database_list.js", "~/Appjs/Database_propertyGrid.js"
              )
                .Include(
             "~/Appjs/GenTask_dataModel.js", "~/Appjs/GenTask_dataStore.js",
             "~/Appjs/GenTask_formPanel.js", "~/Appjs/GenTask_infoPanel.js", "~/Appjs/GenTask_searchPanel.js",
             "~/Appjs/GenTask_gridPanel.js", "~/Appjs/GenTask_list.js", "~/Appjs/GenTask_propertyGrid.js"
              )
               .Include(
             "~/Appjs/Model_dataModel.js", "~/Appjs/Model_dataStore.js",
             "~/Appjs/Model_formPanel.js", "~/Appjs/Model_infoPanel.js", "~/Appjs/Model_searchPanel.js",
             "~/Appjs/Model_gridPanel.js", "~/Appjs/Model_list.js", "~/Appjs/Model_propertyGrid.js"
              )
                /*.Include(
              "~/App/js/model.js"
               )
               .Include(
               "~/App/js/template.js"
               ).Include(
               "~/App/js/company.js"
               ).Include(
               "~/App/js/database.js"
               ).Include(
               "~/App/js/table.js"
               )*/
                );
        }
    }
}
