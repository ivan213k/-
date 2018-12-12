namespace Управление_заказами.Models
{
    class MissingEquipment
    {
        public string Name { get; set; }

        public int NeedCount { get; set; }

        public int Balance { get; set; }

        public int AvalibleInSelectedDateRange { get; set; }

        public int NotEnough
        {
            get { return NeedCount - Balance + AvalibleInSelectedDateRange; }
        }
    }
}
