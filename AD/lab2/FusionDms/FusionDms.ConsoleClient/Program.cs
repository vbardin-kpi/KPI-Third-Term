using System;

using FusionDms.Exceptions;

namespace FusionDms.ConsoleClient
{
    internal static class Program
    {
        public static void Main()
        {
            // Create a database connection. The second parameter allows to create a database file if it's not exists.
            // If this parameter is *false* and DB file is not exists FileNotFoundException will be thrown!
            var fusionConnection = new FusionConnection(
                connectionString: "Db=dbfile.fdb;Access=ReadWrite",
                createIfNotExists: true);

            // Fill the data base.
            // Add 100 records where a key is an integer and a value -- a Guid.
            for (var i = 0; i < 100; i++)
            {
                fusionConnection.Write(i + 1, Guid.NewGuid());
            }

            // Read from the data base
            Console.WriteLine("READ:");
            ReadAndPrintValueById(fusionConnection, 1);
            ReadAndPrintValueById(fusionConnection, 99);
            Console.WriteLine();
            Console.WriteLine();

            // Update a record
            Console.WriteLine("UPDATE:");
            fusionConnection.Update(1, Guid.NewGuid());
            fusionConnection.Update(99, Guid.NewGuid());
            ReadAndPrintValueById(fusionConnection, 1);
            ReadAndPrintValueById(fusionConnection, 99);
            Console.WriteLine();
            Console.WriteLine();

            // Delete a value
            // FusionDms.Exceptions.RecordNotFoundException is expected
            Console.WriteLine("DELETE:");
            try
            {
                fusionConnection.Delete(1);
                ReadAndPrintValueById(fusionConnection, 1);
            }
            catch (RecordNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ReadAndPrintValueById(FusionConnection connection, int id)
        {
            var rec = connection.Read<int, Guid>(id);
            Console.WriteLine("Value for record with id " + id + ": " + rec.ToString("P"));
        }
    }
}