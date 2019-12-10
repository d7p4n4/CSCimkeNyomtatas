using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace CSCimkeNyomtatas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public SqlConnection conn = new SqlConnection("Data Source=80.211.241.82;Integrated Security=False;uid=root;pwd=Sycompla9999*;Initial Catalog=test;");

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            List<Cimke> cimkeList = new List<Cimke>();
            List<Seged3> seged3List = new List<Seged3>();
            List<Segedlap> segedlapList = new List<Segedlap>();
            List<Veszkomp> veszkompList = new List<Veszkomp>();
            List<Termeklap> termeklapList = new List<Termeklap>();
            conn.Open();
            string sqlSeged3 = "select * from Seged3;";
            string sqlSegedlap = "select * from Segedlap;";
            string sqlVeszkomp = "select * from Veszkomp;";
            string sqlTermeklap = "select * from TermeklapU;";

            using (SqlConnection connection = conn)
            {
                SqlCommand cmd = new SqlCommand(sqlSeged3, connection);

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        seged3List.Add(new Seged3()
                        {
                            SAP = Convert.ToString(reader["SAP"]),
                            Pmondatm = Convert.ToString(reader["Pmondatm"]),
                            Hmondatm = Convert.ToString(reader["Hmondatm"]),
                            GHS01 = Convert.ToInt32(reader["GHS01"]),
                            GHS02 = Convert.ToInt32(reader["GHS02"]),
                            GHS03 = Convert.ToInt32(reader["GHS03"]),
                            GHS04 = Convert.ToInt32(reader["GHS04"]),
                            GHS05 = Convert.ToInt32(reader["GHS05"]),
                            GHS06 = Convert.ToInt32(reader["GHS06"]),
                            GHS07 = Convert.ToInt32(reader["GHS07"]),
                            GHS08 = Convert.ToInt32(reader["GHS08"]),
                            GHS09 = Convert.ToInt32(reader["GHS09"]),
                        });

                    }
                }

                SqlCommand cmd2 = new SqlCommand(sqlSegedlap, connection);

                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        segedlapList.Add(new Segedlap()
                        {
                            SAP = Convert.ToString(reader["SAP"]),
                            Ujosszetevokm = Convert.ToString(reader["ujosszetevokm"]),
                            Allergenm = Convert.ToString(reader["ALLERGENM"])
                        });

                    }
                }

                SqlCommand cmdVeszkomp = new SqlCommand(sqlVeszkomp, connection);

                using (SqlDataReader reader = cmdVeszkomp.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        veszkompList.Add(new Veszkomp()
                        {
                            SAP = Convert.ToString(reader["SAPKOD"]),
                            Nev = Convert.ToString(reader["NEV"]),
                            Relacio = Convert.ToString(reader["RELACIO"]),
                            Mennyiseg = Convert.ToString(reader["MENNYISEG"])
                        });

                    }
                }

                SqlCommand cmdTermeklap = new SqlCommand(sqlTermeklap, connection);

                using (SqlDataReader reader = cmdTermeklap.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        termeklapList.Add(new Termeklap()
                        {
                            SAP = Convert.ToString(reader["SAP"]),
                            UNSzam = Convert.ToString(reader["UNSZAM"])
                        });

                    }
                }
            }

            foreach(var termeklap in termeklapList)
            {
                List<Seged3> seged3InnerList = new List<Seged3>();
                List<Segedlap> segedlapInnerList = new List<Segedlap>();
                List<Veszkomp> veszkompInnerList = new List<Veszkomp>();
                List<Termeklap> termeklapInnerList = new List<Termeklap>();

                foreach(var veszkomp in veszkompList)
                {
                    if (veszkomp.SAP.Equals(termeklap.SAP))
                    {
                        veszkompInnerList.Add(veszkomp);
                    }
                }

                foreach (var segedlap in segedlapList)
                {
                    if (segedlap.SAP.Equals(termeklap.SAP))
                    {
                        segedlapInnerList.Add(segedlap);
                    }
                }
                foreach (var seged3 in seged3List)
                {
                    if (seged3.SAP.Equals(termeklap.SAP))
                    {
                        seged3InnerList.Add(seged3);
                    }
                }

                Cimke cimke = new Cimke()
                {
                    SAP = termeklap.SAP,
                    Termeklap = termeklap,
                    VeszkompList = veszkompInnerList,
                    SegedlapList = segedlapInnerList,
                    Seged3List = seged3InnerList
                };

                serialize(cimke, typeof(Cimke), "d:\\Server\\Visual_studio\\output_Xmls\\" + cimke.SAP + ".xml");
            }
        }

        public string serialize(Object taroltEljaras, Type anyType, string path)
        {
            XmlSerializer serializer = new XmlSerializer(anyType);
            var xml = "";

            using (var writer = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer))
                {
                    serializer.Serialize(writer, taroltEljaras);
                    xml = writer.ToString(); // Your XML
                }
            }
            System.IO.File.WriteAllText(path, xml);

            return xml;
        }
    }
}
