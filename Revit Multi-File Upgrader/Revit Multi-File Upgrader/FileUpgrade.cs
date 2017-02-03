using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;

namespace Revit_Multi_File_Upgrader
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class FileUpgrade : IExternalCommand
    {

        public static FileInfo[] files = null;
        public static DirectoryInfo[] subDirs = null;

        public Result Execute(ExternalCommandData cmdData,
            ref string message,
            ElementSet set)
        {

            UIApplication app = cmdData.Application;
            Document doc = null;
            try
            {
                doc = app.ActiveUIDocument.Document;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("There is not currently a project file loaded into this instance of Revit.  Please load a project file and then try running this add-in again.");
                return Result.Failed;
            }

            //Open folder browser.
            FolderBrowserDialog sourceDia = new FolderBrowserDialog();
            FolderBrowserDialog destDia = new FolderBrowserDialog();

            //Start at my Documents.
            sourceDia.RootFolder = Environment.SpecialFolder.MyComputer;
            destDia.RootFolder = Environment.SpecialFolder.MyComputer;
            sourceDia.Description = "Select source folder...";
            destDia.Description = "Select destination folder...";

            if ((sourceDia.ShowDialog() == DialogResult.OK) && (destDia.ShowDialog() == DialogResult.OK))
            {
                //Get selected path.
                string rootFamPath = sourceDia.SelectedPath;
                string[] famLocs = Directory.GetFiles(rootFamPath, "*.rfa", SearchOption.AllDirectories);

                int success = 0;
                int error = 0;

                List<ElementId> eIds = new List<ElementId>();

                // Load familes from Directory into Project space.
                // Families will convert automatically during this process.
                using (Transaction trans = new Transaction(doc, "Family Load"))
                {
                    trans.Start();
                    foreach (string loc in famLocs)
                    {
                        Family fam = null;
                        doc.LoadFamily(loc, out fam);

                        if (fam != null)
                        {
                            eIds.Add(fam.Id);
                        }
                    }
                    trans.Commit();
                }

                try
                {
                    // Save each family into a respective directory
                    // in the user's specified location.
                    foreach (ElementId id in eIds)
                    {
                        Family f = doc.GetElement(id) as Family;
                        Document famDoc = doc.EditFamily(f);
                        string famName = Path.GetFileName(famDoc.PathName);
                        string desLoc = famDoc.PathName.Replace(famName, "");
                        desLoc = desLoc.Replace(sourceDia.SelectedPath, destDia.SelectedPath);

                        if (!Directory.Exists(desLoc))
                        { Directory.CreateDirectory(desLoc); }

                        famDoc.SaveAs(desLoc + f.Name + ".rfa");
                        famDoc.Close(false);
                        success++;
                    }
                }
                catch { error++; }

                System.Windows.Forms.MessageBox.Show("File Upgrade successful!" + Environment.NewLine +
                    "Families Successfully Converted: " + success.ToString() + Environment.NewLine +
                    "Errors: " + error.ToString());
            }
            return Result.Succeeded;
        }
    }
}
