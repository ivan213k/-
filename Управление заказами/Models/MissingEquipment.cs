﻿namespace Управление_заказами.Models
{
    class MissingEquipment
    {
        public string Name { get; set; }

        public int NeedCount { get; set; }

        public int Balance { get; set; }

        public int AvalibleInSelectedDateRange { get; set; }

        public bool IsPartnerHave { get; set; }

        public string PartnerName { get; set; }

        public int NotEnough
        {
            get { return NeedCount - (Balance + AvalibleInSelectedDateRange); }
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
