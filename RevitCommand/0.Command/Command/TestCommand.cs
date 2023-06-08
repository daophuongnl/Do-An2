using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Model.Form;
using System;
using System.Linq;
using System.Collections.Generic;
using Utility;
using Autodesk.Revit.DB.Plumbing;
using System.Net;

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
            // tạo mặt phẳng qua điểm giao
            XYZ normal = XYZ.BasisZ;
            XYZ origin = projectPoint;
            Plane plane1 = Plane.CreateByNormalAndOrigin(normal, origin);

            var endpoint2 = plane1.GetProjectPoint(sprinklerConnectorOrigin);
            
            
            // thực hiện lệnh
            //okok
            using (var transaction = new Transaction(doc,"Pipe sprinkler"))
            {
                transaction.Start();

                var pipeTypeId = mainPipe.PipeType.Id;
                var levelId = mainPipe.LookupParameter("Reference Level").AsElementId();
                var endpoint1 = sprinklerConnectorOrigin + sprinklerConnectorDirection * 200.0.milimeter2Feet();
                var fisrtPipe = Pipe.Create(doc, pipeTypeId, levelId, sprinklerConnector, endpoint1);

                var diameter = 32.0.milimeter2Feet();
                fisrtPipe.LookupParameter("Diameter").Set(diameter);



                //Pipe pipe2 = Pipe.Create(doc, pipeTypeId, levelId, projectPoint, endpoint2);

                transaction.Commit();

            }

            
            //var 
        }

        //public void connector2Pipe(Pipe firstPipe, Pipe secondPipe)
        //{
        //    var connector1 = firstPipe.ConnectorManager.UnusedConnectors.Cast<Connector>();
        //    var connector2 = secondPipe.ConnectorManager.UnusedConnectors.Cast<Connector>();
        //    Connector? connector1 = null;
        //    Connector? connector2 = null;
            
        //    foreach (var conn1 in connector1)
        //    {
        //        if (connector1 !=null && connector2 != null)
        //        {
        //            break;
        //        }  
        //        var origin1 = conn1.Origin;
        //        foreach( var conn2 in connector2)
        //        {
        //            var origin2= conn2.Origin;
        //            if (origin1.IsEqual(origin2))
        //            {
        //                connector1 = conn1;
        //                connector2 = conn2;
        //                break;
        //            }
        //        }
        //    }
        //    if (connector1== null) || connector2 == null )
        //    {
        //        return;
        //    }
        //    doc.Create.NewElbowFitting(connector1, connector2);
        //    doc.Create.NewTeeFitting()

        //}

    }

    [Transaction(TransactionMode.Manual)]
    public class TestCommand2 : RevitCommand
    {
        public override void Execute()
        {
            var pipe = sel.PickElement<Pipe>();
            var pipeSolid = pipe.GetEntElement()!.EntSolid.Solid;

            var linkInstance = sel.PickElement<RevitLinkInstance>();

            var linkDoc = linkInstance.GetLinkDocument();
            var linkWalls = new FilteredElementCollector(linkDoc).OfClass(typeof(Wall)).Cast<Wall>().ToList();

            using (var transaction = new Transaction(doc, "Create "))
            {
                transaction.Start();

                foreach (var wall in linkWalls)
                {
                    var eWall = wall.GetEntElement()!;
                    eWall.LinkTransform = linkInstance.GetTransform();

                    var solid = eWall.EntSolid[Entity.SolidCode.Link];
                    if (solid!.IsIntersect(pipeSolid))
                    {
                        var intersectSolid = BooleanOperationsUtils.ExecuteBooleanOperation(solid, pipeSolid, BooleanOperationsType.Intersect);
                        var intersectPoint = intersectSolid.ComputeCentroid();

                        var modelLine = Line.CreateBound(intersectPoint, intersectPoint + XYZ.BasisX * 10.0.meter2Feet()).CreateModel();
                        sel.SetElement(modelLine);
                    }
                }

                transaction.Commit();
            }
        }
    }
}