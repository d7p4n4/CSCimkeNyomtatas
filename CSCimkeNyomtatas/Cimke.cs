using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCimkeNyomtatas
{
    public class Cimke
    {
        public string SAP { get; set; }
        public Termeklap Termeklap {get;set; }
        public List<Veszkomp> VeszkompList { get; set; }
        public List<Segedlap> SegedlapList { get; set; }
        public List<Seged3> Seged3List { get; set; }
    }
}
