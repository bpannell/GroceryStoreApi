using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.External
{
    public interface IDatabaseRepository
    {
        Task<Database> ReadDatabase();
        Task SaveDatabase(Database database);
    }
}
