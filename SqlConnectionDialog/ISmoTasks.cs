﻿using System.Collections.Generic;

namespace IwAG.Win.UI.Controls
{
    public interface ISmoTasks
    {
        IEnumerable<string> SqlServers {get;}
        List<string> GetDatabases(SqlConnectionString connectionString);
        List<DatabaseTable> GetTables(SqlConnectionString connectionString);
    }
}
