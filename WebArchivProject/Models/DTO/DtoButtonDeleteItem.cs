namespace WebArchivProject.Models.DTO
{
    public class DtoButtonDeleteItem
    {
        public int DeletedItemId { get; set; }
        public string ItemType { get; set; }
        public bool Disabled { get; set; }

        public string ItemTrigger { get; set; }

        public string CollapseTarget { get; set; }
        public string CollapseControls { get; set; }
        public string CollapseTrigger { get; set; }
    }
}
