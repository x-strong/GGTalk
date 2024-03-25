using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GGTalk.Models
{
    public class ListItem<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
        public ListItem()
        {
        }
        public ListItem(string pKey, T pValue)
        {
            this.Key = pKey;
            this.Value = pValue;
        }
        public override string ToString()
        {
            return this.Key;
        }
    }
}
