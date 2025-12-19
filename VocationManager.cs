using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace PartyDatabase
{
    class VocationManager
    {
        private static readonly string _databasePathFile = Path.Combine(Directory.GetCurrentDirectory(), "Data", "vocations.db");
        private static readonly string _connectingString = $"Data Source={_databasePathFile}";

        public static void VerifyDatabaseIsCreated()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_databasePathFile));
        }
    }
}