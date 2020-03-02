using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    class DevicesSchema
    {
        public String Name, ID, Location;
        public List<ConnectorSchema> Connectors = new List<ConnectorSchema>();
    }

    class ConnectorSchema
    {
        public String Type, id, Name;

        public ConnectorSchema(String Type, String ID, String Name)
        {
            this.Type = Type;
            this.id = ID;
            this.Name = Name;
        }
    }
}
