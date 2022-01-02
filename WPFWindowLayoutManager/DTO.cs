using System.Collections.Generic;

namespace Abraham.WPFWindowLayoutManager
{
	public class DTO
    {
        public List<LayoutElement> Elements { get; set; }

        public DTO()
        {
            Elements = new List<LayoutElement>();
            Listbox1Columns = new List<int>();
            Listbox2Columns = new List<int>();
            Listbox3Columns = new List<int>();
        }

        public int          Splitter1Distance   { get; set; }
        public List<int>    Listbox1Columns     { get; set; }
        public List<int>    Listbox2Columns     { get; set; }
        public List<int>    Listbox3Columns     { get; set; }
    }
}
