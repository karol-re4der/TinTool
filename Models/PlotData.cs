using OxyPlot;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.DataStructures
{
    public class PlotData
    {
        public PlotData()
        {
            this.Title = "Example 2";
            this.Points = new List<DataPoint>();
        }

        public string Title { get; set; }
        

        public IList<DataPoint> Points { get; private set; }
    }
}
