using GroceryStoreAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.External
{
    public class DatabaseRespository : IDatabaseRepository
    {
        private const string jsonFile = "./database.json";

        public DatabaseRespository()
        {
        }

        public async Task<Database> ReadDatabase()
        {
            var jsonString = await File.ReadAllTextAsync(jsonFile);
            return JsonConvert.DeserializeObject<Database>(jsonString);
        }

        public async Task SaveDatabase(Database database)
        {
            var jsonString = JsonConvert.SerializeObject(database);
            await File.WriteAllTextAsync(jsonFile, jsonString);
        }
    }
}
