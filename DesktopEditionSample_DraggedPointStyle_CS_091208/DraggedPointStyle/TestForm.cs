﻿using System;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.IO;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.DesktopEdition;

namespace  DraggedPointStyle
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            winformsMap1.MapUnit = GeographyUnit.DecimalDegree;
            winformsMap1.CurrentExtent = new RectangleShape(-97.7591, 30.3126, -97.7317, 30.2964);
            winformsMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.FromArgb(255, 198, 255, 255));

            //Displays the World Map Kit as a background.
            ThinkGeo.MapSuite.DesktopEdition.WorldMapKitWmsDesktopOverlay worldMapKitDesktopOverlay = new ThinkGeo.MapSuite.DesktopEdition.WorldMapKitWmsDesktopOverlay();
            winformsMap1.Overlays.Add(worldMapKitDesktopOverlay);

            string fileName1 = @"..\..\data\polygon.txt";
            StreamReader sr1 = new StreamReader(fileName1);

            string fileName2 = @"..\..\data\line.txt";
            StreamReader sr2 = new StreamReader(fileName2);

            //DragtInteractiveOverlay for setting the PointStyles of the control points and dragged points.
            DragInteractiveOverlay dragInteractiveOverlay = new DragInteractiveOverlay();
            dragInteractiveOverlay.EditShapesLayer.InternalFeatures.Add("Polygon", new Feature(BaseShape.CreateShapeFromWellKnownData(sr1.ReadLine())));
            dragInteractiveOverlay.EditShapesLayer.InternalFeatures.Add("MultiLine", new Feature(BaseShape.CreateShapeFromWellKnownData(sr2.ReadLine())));

            //Sets the PointStyle for the non dragged control points.
            dragInteractiveOverlay.ControlPointStyle = new PointStyle(PointSymbolType.Circle, new GeoSolidBrush(GeoColor.StandardColors.PaleGoldenrod), new GeoPen(GeoColor.StandardColors.Black), 8);
            //Sets the PointStyle for the dragged control points.
            dragInteractiveOverlay.DraggedControlPointStyle = new PointStyle(PointSymbolType.Circle, new GeoSolidBrush(GeoColor.StandardColors.Red), new GeoPen(GeoColor.StandardColors.Orange,2), 10);

            dragInteractiveOverlay.CanAddVertex = false;
            dragInteractiveOverlay.CanDrag = false;
            dragInteractiveOverlay.CanRemoveVertex = false;
            dragInteractiveOverlay.CanResize = false;
            dragInteractiveOverlay.CanRotate = false;
            dragInteractiveOverlay.CalculateAllControlPoints();

            winformsMap1.EditOverlay = dragInteractiveOverlay;
            
            winformsMap1.Refresh();
        }

        private void winformsMap1_MouseMove(object sender, MouseEventArgs e)
        {
            //Displays the X and Y in screen coordinates.
            statusStrip1.Items["toolStripStatusLabelScreen"].Text = "X:" + e.X + " Y:" + e.Y;

            //Gets the PointShape in world coordinates from screen coordinates.
            PointShape pointShape = ExtentHelper.ToWorldCoordinate(winformsMap1.CurrentExtent, new ScreenPointF(e.X, e.Y), winformsMap1.Width, winformsMap1.Height);

            //Displays world coordinates.
            statusStrip1.Items["toolStripStatusLabelWorld"].Text = "(world) X:" + Math.Round(pointShape.X, 4) + " Y:" + Math.Round(pointShape.Y, 4);
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
