using CableSchedule;
using CsvHelper;
using Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LucidChart_CSV_Reader
{
    class Program
    {
        static void Main(string[] args)

        {
            String file_dest = "./test.csv";
            Schedule cableSchedule = new Schedule();
            List<LayoutSchema> lucidForm = new List<LayoutSchema>();
            List<DevicesSchema> deviceList = new List<DevicesSchema>();
            using (TextReader reader = File.OpenText(file_dest))
            {

                CsvReader csv = new CsvReader(reader);
                csv.Configuration.Delimiter = ",";
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => Regex.Replace(header, @"\s", string.Empty); //Ignores spaces
                while (csv.Read())
                {
                    LayoutSchema Record = csv.GetRecord<LayoutSchema>();
                    lucidForm.Add(Record);
                }

                //Generate Cable Schedule
                int cableCounter = 0;
                int deviceCounter = 0;
                for (int i = 0; i < lucidForm.Count; i++) //Scan through lucid CSV form items
                {

                    if (lucidForm[i].Name == "Swim Lane")   //Get devices
                    {
                        deviceList.Add(new DevicesSchema());
                        deviceList[deviceCounter].ID = lucidForm[i].Id;
                        deviceList[deviceCounter].Name = lucidForm[i].TextArea1;
                        deviceList[deviceCounter].Location = lucidForm[i].location;

                        string thisName = deviceList[deviceCounter].Name;
                        string thisId = deviceList[deviceCounter].ID;
                        Console.WriteLine("Device {0} registered as Device {1}", thisName, thisId);

                       deviceCounter++;
                    }
                }

                cableCounter = 0;
                deviceCounter = 0;
                for (int i = 0; i < lucidForm.Count; i++) //Scan through lucid CSV form items
                {
                    if (lucidForm[i].ContainedBy != "") // associate connectors with device
                    {
                        // Delimit string of :
                        String thisConnector = lucidForm[i].connector;
                        Console.Write("Line {0} {1}", lucidForm[i].Id, thisConnector);
                        Console.Write(" Is associated with ");
                        string data = lucidForm[i].ContainedBy;
                        string[] words = Regex.Split(data,":");
                        //Left number is ID of device
                        int.TryParse(words[0], out int leftPartial); //convert to integer, to remove unwanted 0's
                        int.TryParse(words[1], out int rightPartial); //convert to integer, to remove unwanted 0's
                        //Right is to be ignored
                        Console.Write("Device ");
                        Console.Write(leftPartial);
                        Console.WriteLine();

                        int deviceIndex = deviceList.FindIndex(DevicesSchema => DevicesSchema.ID == leftPartial.ToString());
                        
                        if (deviceIndex >= 0)
                        {
                            //Add connector to device
                            Console.WriteLine("Adding Port to {0}", deviceList[deviceIndex].Name);
                            deviceList[deviceIndex].Connectors.Add(new ConnectorSchema(Type: thisConnector, ID: lucidForm[i].Id, Name: lucidForm[i].TextArea1));
                            deviceList[deviceIndex].Connectors.Add(new ConnectorSchema(Type: thisConnector, ID: lucidForm[i].Id, Name: lucidForm[i].TextArea1));
                        }
                        else
                        {
                            Console.WriteLine("Device {0} Not recognised", deviceIndex);
                        }

                        Console.WriteLine();
                    }
                }
                    
                    for (int i = 0; i < lucidForm.Count; i++) //Scan trhough all items
                {
                    
                    if (lucidForm[i].Name == "Line") //Is a connection type
                    {
                        //LineSource will always 
                        int.TryParse(lucidForm[i].LineSource, out int lineStart);
                        int.TryParse(lucidForm[i].LineDestination, out int lineEnd);

                        cableSchedule.Cable.Add(new ExportSchmea());
                        cableSchedule.Cable[cableCounter].ID = lucidForm[i].TextArea1; // ID for cable
                        Console.WriteLine("ID: {0}", cableSchedule.Cable[cableCounter].ID);

                        // START DEVICE
                        //
                        ///
                        ///

                        //Linesource
                        int LineIndex = lucidForm.FindIndex(LayoutSchema => LayoutSchema.Id == lucidForm[i].LineSource);

                        //Container
                        string data = lucidForm[LineIndex].ContainedBy;
                        string[] words = Regex.Split(data, ":");
                        //Left number is ID of device
                        int.TryParse(words[0], out int leftPartial); //convert to integer, to remove unwanted 0's

                        
                        int deviceIndex = deviceList.FindIndex(obj => obj.ID == leftPartial.ToString());

                        if (deviceIndex >= 0)
                        {
                            DevicesSchema thisDevice = deviceList[deviceIndex];

                            cableSchedule.Cable[cableCounter].Start = thisDevice.Name;
                            Console.WriteLine("START: {0}",deviceList[deviceIndex].Name);

                            cableSchedule.Cable[cableCounter].StartLocation = thisDevice.Location;
                            Console.WriteLine("LOCATION: {0}", deviceList[deviceIndex].Location);

                            int connecterIndex = thisDevice.Connectors.FindIndex(obj => obj.id == lineStart.ToString());
                            cableSchedule.Cable[cableCounter].StartConectorName = thisDevice.Connectors[connecterIndex].Name;
                            Console.WriteLine("START CONNECTOR NAME: {0} ", thisDevice.Connectors[connecterIndex].Name);

                            connecterIndex = thisDevice.Connectors.FindIndex(obj => obj.id == lineStart.ToString());
                            cableSchedule.Cable[cableCounter].StartConector = thisDevice.Connectors[connecterIndex].Type;
                            Console.WriteLine("START CONNECTOR: {0} ", thisDevice.Connectors[connecterIndex].Type);
                        }
                        ///////////////////////////////////////////////////////////////////////////////////////////////////
                      
                        // END DEVICE
                        ///
                        ///
                        ///
                        //
                        //
                        LineIndex = lucidForm.FindIndex(obj => obj.Id == lucidForm[i].LineDestination );

                        //Container
                        data = lucidForm[LineIndex].ContainedBy;
                        words = Regex.Split(data, ":");
                        //Left number is ID of device
                        int.TryParse(words[0], out leftPartial); //convert to integer, to remove unwanted 0's

                        Console.WriteLine(LineIndex);
                        deviceIndex = deviceList.FindIndex(obj => obj.ID == leftPartial.ToString());
                        
                 

                        if (deviceIndex >= 0)
                        {

                            DevicesSchema thisDevice = deviceList[deviceIndex];

                            cableSchedule.Cable[cableCounter].End = thisDevice.Name;
                            Console.WriteLine("END: {0}", deviceList[deviceIndex].Name);

                            cableSchedule.Cable[cableCounter].EndLocation = thisDevice.Location;
                            Console.WriteLine("LOCATION: {0}", deviceList[deviceIndex].Location);

                            int connecterIndex = thisDevice.Connectors.FindIndex(obj => obj.id == lineEnd.ToString());
                            cableSchedule.Cable[cableCounter].EndConnectorName = thisDevice.Connectors[connecterIndex].Name;
                            Console.WriteLine("END CONNECTOR NAME: {0} ", thisDevice.Connectors[connecterIndex].Name);

                            connecterIndex = thisDevice.Connectors.FindIndex(obj => obj.id == lineEnd.ToString());
                            cableSchedule.Cable[cableCounter].EndConnector = thisDevice.Connectors[connecterIndex].Type;
                            Console.WriteLine("END CONNECTOR: {0} ", thisDevice.Connectors[connecterIndex].Type);
                        }


                        cableCounter++;
                        /////////////////////////////////////////////////////////////////////////////
                    }
                    Console.WriteLine();
                }

            }
            using (var writer = new StreamWriter("./file.csv"))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(cableSchedule.Cable);
            }


            Console.ReadKey();
        }
    }
}
