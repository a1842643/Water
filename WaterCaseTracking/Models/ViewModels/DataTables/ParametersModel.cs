namespace WaterCaseTracking.Models.ViewModels.DataTables
{
    public class Parameters
    {
        private string _sSortDir_0;
        public int sEcho { get; set; }
        public int iColumns { get; set; }
        public string sColumns { get; set; }
        public int iDisplayStart { get; set; }
        public int iDisplayLength { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 {
            get { return string.IsNullOrEmpty(_sSortDir_0) || _sSortDir_0.ToLower() == "desc" ? "desc" : "asc"; }
            set { _sSortDir_0 = value; }
        }
    }
}