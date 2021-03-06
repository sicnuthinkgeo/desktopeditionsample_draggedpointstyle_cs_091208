﻿using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.DesktopEdition;

namespace DraggedPointStyle
{
    class DragInteractiveOverlay : EditInteractiveOverlay
    {
        private PointStyle controlPointStyle;
        private PointStyle draggedControlPointStyle;

        public PointStyle ControlPointStyle
        {
            get { return controlPointStyle; }
            set { controlPointStyle = value; }
        }

        public PointStyle DraggedControlPointStyle
        {
            get { return draggedControlPointStyle; }
            set { draggedControlPointStyle = value; }
        }

        //Overrides the DrawCore function.
        protected override void DrawCore(GeoCanvas canvas)
        {
            //Draws the Edit Shapes as default.
            Collection<SimpleCandidate> labelsInAllLayers = new Collection<SimpleCandidate>();
            EditShapesLayer.Open();
            EditShapesLayer.Draw(canvas, labelsInAllLayers);
            canvas.Flush();
           
            //Draws the control points. 
            ExistingControlPointsLayer.Open();
            Collection<Feature> controlPoints = ExistingControlPointsLayer.FeatureSource.GetAllFeatures(ReturningColumnsType.AllColumns);

            //Loops thru the control points.
            foreach (Feature feature in controlPoints)
            {
                //Looks at the value of "state" to draw the control point as dragged or not.
                if (feature.ColumnValues["state"] != "selected")
                {
                    Feature[] features = new Feature[1] { feature };
                    controlPointStyle.Draw(features, canvas, labelsInAllLayers, labelsInAllLayers);
                }
                else
                {
                    Feature[] features = new Feature[1] { feature };
                    draggedControlPointStyle.Draw(features, canvas, labelsInAllLayers, labelsInAllLayers);
                }
            }
        }
     }
 }
