using System;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace Revit_Multi_File_Upgrader
{
    public class Entry : IExternalApplication
    {
        static AddInId _appId = new Autodesk.Revit.DB.AddInId(new Guid("502fe383-2648-4e98-adf8-5e6047f9dc37"));
        static readonly string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private AppDocEvents appEv;

        public Result OnStartup(UIControlledApplication app)
        {
            //Load Menu and Event Handlers.
            AddMenu(app);
            AddAppDocEvents(app.ControlledApplication);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            RemoveAppDocEvents();

            return Result.Succeeded;
        }

        private void AddMenu(UIControlledApplication app)
        {
            RibbonPanel ribbonPanel = app.CreateRibbonPanel("Revit Family Utilities");
            PulldownButtonData data = new PulldownButtonData("Options", "Revit Family Utilities");

            RibbonItem item = ribbonPanel.AddItem(data);
            PulldownButton opt = item as PulldownButton;

            opt.AddPushButton(new PushButtonData("File Upgrade", "File Upgrade", ExecutingAssemblyPath, "FamilyUtilities.FileUpgrade"));
            opt.AddPushButton(new PushButtonData("Parameter Manager", "Parameter Manager", ExecutingAssemblyPath, "FamilyUtilities.ParameterManager"));
        }

        private void AddAppDocEvents(Autodesk.Revit.ApplicationServices.ControlledApplication app)
        {
            appEv = new AppDocEvents(app);
            appEv.EnableEvents();
        }

        private void RemoveAppDocEvents()
        {
            appEv.DisableEvents();
        }
    }
}
