namespace Управление_заказами.Models.DataBase
{
    class EquipmentInStock : Equipment
    {
        public string ImageUrl { get; set; }

        public int TotalCount { get; set; }

        public string Note { get; set; }
    }
}
