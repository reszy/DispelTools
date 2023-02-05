using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.Components.PictureDisplay
{
    internal class ImageZoom
    {
        private int currentZoomStepNumber;
        private readonly double zoomStep;
        private double[] zoomStepsTable;

        public ImageZoom()
        {
            currentZoomStepNumber = 0;
            zoomStep = 0.005;
            zoomStepsTable = new double[] { 1 };
        }

        public void InitZoom(double defaultZoomSetting)
        {
            var zoomTable = new List<double>
                {
                    defaultZoomSetting, 1, 1.25, 1.5,2, 3, 4, 5, 8, 10
                };
            if (defaultZoomSetting < 1)
            {
                double zoom = defaultZoomSetting;
                for (int i = 1; i < 10; zoom *= 2, i++)
                {
                    if (1 - zoom > zoomStep)
                    {
                        zoomTable.Add(zoom);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            zoomTable.Sort();
            zoomTable = zoomTable.Distinct().ToList();

            zoomStepsTable = zoomTable.ToArray();
            currentZoomStepNumber = zoomTable.IndexOf(defaultZoomSetting);
        }

        public double GetZoom() => zoomStepsTable[currentZoomStepNumber];

        public void StepUp()
        {
            if (currentZoomStepNumber < zoomStepsTable.Length - 1)
                currentZoomStepNumber++;
        }
        public void StepDown()
        {
            if (currentZoomStepNumber > 0)
                currentZoomStepNumber--;
        }

        public void Reset()
        {
            currentZoomStepNumber = 0;
            zoomStepsTable = new double[] { 1 };
        }
    }
}
