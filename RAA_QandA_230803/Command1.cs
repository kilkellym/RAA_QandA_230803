#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

#endregion

namespace RAA_QandA_230803
{
    [Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // Your code goes here
            ElementId testId = GetProjectParameterId(doc, "TEST PARAM");

            return Result.Succeeded;
        }

        internal ElementId GetProjectParameterId(Document doc, string name)
        {
            ParameterElement pElem = new FilteredElementCollector(doc)
                .OfClass(typeof(ParameterElement))
                .Cast<ParameterElement>()
                .Where(e => e.Name.Equals(name))
                .FirstOrDefault();

            return pElem?.Id;
        }

        internal List<Element> GetMultipleCats(Document doc)
        {
            List<Element> returnElements = new List<Element>();

            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfCategory(BuiltInCategory.OST_Walls);

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_Columns);

            returnElements.AddRange(collector1.ToList());
            returnElements.AddRange(collector2.ToList());

            return returnElements;
        }

        internal List<Element> GetMultipleCats2(Document doc, List<BuiltInCategory> catList)
        {
            List<Element> returnElements = new List<Element>();

            foreach(BuiltInCategory cat in catList)
            {
                FilteredElementCollector collector1 = new FilteredElementCollector(doc);
                collector1.OfCategory(cat);

                returnElements.AddRange(collector1.ToList());
            }
            
            return returnElements;
        }

        internal List<Element> GetMultipleCats3(Document doc, List<BuiltInCategory> catList)
        {
            ElementMulticategoryFilter filter = new ElementMulticategoryFilter(catList);

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.WherePasses(filter);

            return collector.ToList();
        }

        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData1.Data;
        }
    }
}
