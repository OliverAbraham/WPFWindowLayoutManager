using System.Collections.Generic;

namespace Abraham.WPFWindowLayoutManager
{
    public class LayoutElement
    {
        public string    Key     { get; set; } = "";
        public int       Left    { get; set; }
        public int       Top     { get; set; }
        public int       Width   { get; set; }
        public int       Height  { get; set; }
        public int       Value   { get; set; }
        public List<int> Values  { get; set; }
        public string    State   { get; set; } = "";
        
        public LayoutElement()
        {
            Values = new List<int>();
        }

        public LayoutElement(string key)
        {
            Key = key;
            Values = new List<int>();
        }

        public override string ToString()
        {
            return $"{Key} {Left}/{Top}/{Width}/{Height}/{State}   {Values.Count} values";
        }
    }
}
