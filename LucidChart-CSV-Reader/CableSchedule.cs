using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableSchedule
{
    class LayoutSchema
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String ContainedBy { get; set; }
        public String LineSource { get; set; }
        public String LineDestination { get; set; }
        public String connector { get; set; }
        public String TextArea1 { get; set; }


    }
    class ExportSchmea
    {
        public String ID { get; set; }
        public String Start { get; set; }
        public String StartLocation { get; set; }
        public String StartConector { get; set; }
        public String StartConectorName { get; set; }
        public String End { get; set; }
        public String EndLocation { get; set; }
        public String EndConnector { get; set; }
        public String EndConnectorName { get; set; }

    }
    class Schedule
    {
        public List<ExportSchmea> Cable = new List<ExportSchmea>();
        public Schedule()
        {

        }
    }
}
