namespace Управление_заказами.Models.DataBase
{
    class EquipmentInStock : Equipment
    {
        private int _totalCount;

        public string ImageUrl { get; set; }

        public int TotalCount
        {
            get => _totalCount;
            set
            {
                if (value >= 0)
                {

                    if (value > _totalCount)
                    {
                        Count += value - _totalCount;
                    }

                    if (value < _totalCount)
                    {
                        Count -= _totalCount - value;
                    }
                    _totalCount = value;
                }
            }
        }

        public string Note { get; set; }
    }
}
