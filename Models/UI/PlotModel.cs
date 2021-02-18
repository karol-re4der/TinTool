using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.UI
{
    public class PlotModel
    {
        public PlotModel()
        {
            Title = "Example 2";
            Points = new List<DataPoint>();
        }

        public string Title { get; set; }


        public IList<DataPoint> Points { get; private set; }
    }
}
