using System;
using Autodesk.Revit.ApplicationServices;

namespace Revit_Multi_File_Upgrader
{
    public class AppDocEvents
    {
        private ControlledApplication m_app;

        public AppDocEvents(ControlledApplication app)
        {
            m_app = app;
        }

        public void EnableEvents()
        {
            m_app.DocumentClosed += new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(m_app_DocumentClosed);
            m_app.DocumentOpened += new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(m_app_DocumentOpened);
            m_app.DocumentSaved += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedEventArgs>(m_app_DocumentSaved);
            m_app.DocumentSavedAs += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs>(m_app_DocumentSavedAs);
        }

        public void DisableEvents()
        {
            m_app.DocumentClosed -= new EventHandler<Autodesk.Revit.DB.Events.DocumentClosedEventArgs>(m_app_DocumentClosed);
            m_app.DocumentOpened -= new EventHandler<Autodesk.Revit.DB.Events.DocumentOpenedEventArgs>(m_app_DocumentOpened);
            m_app.DocumentSaved -= new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedEventArgs>(m_app_DocumentSaved);
            m_app.DocumentSavedAs -= new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs>(m_app_DocumentSavedAs);
        }

        void m_app_DocumentSavedAs(object sender, Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs e)
        {
        }

        void m_app_DocumentSaved(object sender, Autodesk.Revit.DB.Events.DocumentSavedEventArgs e)
        {
        }

        void m_app_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
        }

        void m_app_DocumentClosed(object sender, Autodesk.Revit.DB.Events.DocumentClosedEventArgs e)
        {
        }

    }
}
