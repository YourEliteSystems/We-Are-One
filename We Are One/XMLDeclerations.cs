using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace We_Are_One
{
    public class XMLDeclerations
    {
        [XmlAttribute]
        public string Station
        {
            get;
            set;
        }

        [XmlAnyAttribute]
        public string Url
        {
            get;
            set;
        }
    }
}
