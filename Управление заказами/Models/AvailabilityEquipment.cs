using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Управление_заказами.Models
{
    class AvailabilityEquipment
    {
        public string Name { get; set; }
        public int NeedCount { get; set; }
        public int Balance { get; set; }
        public int AvalibleInSelectedDateRange { get; set; }
        public bool IsEnough { get; set; }
        public string Result { get; set; }
        public int TotalCount { get; set; }
        public string ImageUrl { get; set; }

        public int NotEnough
        {
            get
            {
                int notEnough = NeedCount - (Balance + AvalibleInSelectedDateRange);
                return notEnough > 0 ? notEnough : 0;
            }
        }

        public int TotalAvalibleCount
        {
            get { return Balance + AvalibleInSelectedDateRange; }
        }

        public int WillRemainInStock
        {
            get
            {
                int remainCount = (Balance + AvalibleInSelectedDateRange) - NeedCount;
                return remainCount > 0 ? remainCount : 0;
            }
        }
    }
}
