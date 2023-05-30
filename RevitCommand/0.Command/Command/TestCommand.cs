using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Model.Form;
using System;
using System.Linq;
using System.Collections.Generic;
using Utility;
using Autodesk.Revit.DB.Plumbing;

namespace Model.RevitCommand
{
    [Transaction(TransactionMode.Manual)]
    public class TestCommand : RevitCommand
    {
        public override void Execute()
        {
            var mainPipe = sel.PickElement<Pipe>();
            var sprinkler = sel.PickElement<FamilyInstance>();

            var sprinklerConnector = sprinkler.MEPModel.ConnectorManager.UnusedConnectors.Cast<Connector>()
                .First(x => x.CoordinateSystem.BasisZ.IsParallel(XYZ.BasisZ));

            var sprinklerConnectorOrigin = sprinklerConnector.Origin;

            var sprinklerConnectorDirection = sprinklerConnector.CoordinateSystem.BasisZ;

            var mainLocationLine = ((mainPipe.Location as LocationCurve)!.Curve as Line)!;

            var projectPoint = mainLocationLine.GetProjectPoint(sprinklerConnectorOrigin);

            XYZ normal = XYZ.BasisZ;
            XYZ origin = projectPoint;
            Plane plane1 = Plane.CreateByNormalAndOrigin(normal, origin);

           PointOnPlane point1 = doc.Application.Create.NewPointOnPlane(plane1,UV.Zero, UV

         
            
            using (var transaction = new Transaction(doc,"Pipe sprinkler"))
            {
                transaction.Start();

                var pipeTypeId = mainPipe.PipeType.Id;
                var levelId = mainPipe.LookupParameter("Reference Level").AsElementId();
                var endpoint1 = sprinklerConnectorOrigin + sprinklerConnectorDirection * 100.0.milimeter2Feet();
                var fisrtPipe = Pipe.Create(doc, pipeTypeId, levelId, sprinklerConnector, endpoint1);

                var diameter = 32.0.milimeter2Feet();
                fisrtPipe.LookupParameter("Diameter").Set(diameter);

                

                transaction.Commit();

            }

            
            //var 
        }
    }
}