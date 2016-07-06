using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeographicUseCmd.ReadWriteCsv;
using GeographicLib.Tiles;

namespace GeographicUseCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Howdy!, Ready!");
            Console.ReadKey();
            Console.WriteLine("Go! Generating...");
            double numRecords = 0;
            numRecords = WriteQuadKeysAndCoordinates("WorldQuadKeysAndCoordinates.csv");
            Console.WriteLine("Finished!");
            Console.WriteLine("Generated {0} records (Not counting the Header)", numRecords);
            Console.ReadKey();
        }

        public static double WriteQuadKeysAndCoordinates(string fileName)
        {
            // Write to CSV file
            using (CsvFileWriter writer = new CsvFileWriter(fileName))
            {
                //Generate all QuadKeys from Coordinates based on all real max/min Coordinates (With no decimals)
                // Min. Coordinate: (-90, -180)
                // Max. Coordinate: (90, 180)

                //Write Headers
                CsvRow rowHeader = new CsvRow();
                rowHeader.Add(String.Format("Latitude"));
                rowHeader.Add(String.Format("Longitude"));
                rowHeader.Add(String.Format("QuadKey"));
                writer.WriteRow(rowHeader);

                double numRecords = 0;
                for (double latitude = -90; latitude <= 90; latitude++)
                {
                    for (double logitude = -180; logitude <= 180; logitude++)
                    {
                        CsvRow row = new CsvRow();
                        string currentQuadKey = GeoTileTool.GeoCoordinateToQuadKey(latitude, logitude, 19);
                        row.Add(String.Format("{0}", latitude));
                        row.Add(String.Format("{0}", logitude));
                        row.Add(String.Format("{0}", currentQuadKey));

                        writer.WriteRow(row);
                        numRecords++;
                    }    
                }

                return numRecords;
            }
        }

        public static void WriteTest(string fileName)
        {
            // Write sample data to CSV file
            using (CsvFileWriter writer = new CsvFileWriter(fileName))
            {
                for (int i = 0; i < 100; i++)
                {
                    CsvRow row = new CsvRow();
                    for (int j = 0; j < 5; j++)
                        row.Add(String.Format("Column{0}", j));
                    writer.WriteRow(row);
                }
            }
        }

        public static void ReadTest(string fileName)
        {
            // Read sample data from CSV file
            using (CsvFileReader reader = new CsvFileReader(fileName))
            {
                CsvRow row = new CsvRow();
                while (reader.ReadRow(row))
                {
                    foreach (string s in row)
                    {
                        Console.Write(s);
                        Console.Write(" ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
