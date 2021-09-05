using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace timeov.Functions.Entities
{
    public class TimeEntity : TableEntity
    {
        public int id { get; set; }

        public DateTime dateTime { get; set; }

        public byte type { get; set; }

        public bool isConsolidated { get; set; }

    }
}
