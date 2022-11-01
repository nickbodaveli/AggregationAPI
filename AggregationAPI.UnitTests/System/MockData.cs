using AggregationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AggregationAPI.UnitTests.System
{
    public static class MockData
    {
        public static List<Dataset> MockDatasets()
        {
            var datasets = new List<Dataset>()
               {
                   new Dataset()
                   {
                       Region = "Test Region 1",
                       ObjName = "Test Name 1",
                       ObjNumber = 1,
                       ObjType = "Test Object Type 1",
                       PPlus = 0.5,
                       PMinus = 0.1,
                       PlTime = DateTime.Now
                   },
                   new Dataset()
                   {
                       Region = "Test Region 2",
                       ObjName = "Test Name 2",
                       ObjNumber = 2,
                       ObjType = "Test Object Type 2",
                       PPlus = 0.7,
                       PMinus = 0.2,
                       PlTime = DateTime.Now
                   }
               };
            return datasets;
        }
    }
}
