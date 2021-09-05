using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace timeov.Functions.Entities
{
    public class ConsolidatedEntity : TableEntity
    {
        public int employeeId { get; set; }

        public DateTime dateTime { get; set; }

        public int timeWorked { get; set; }
    }
}
