using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Xml_fajlkezeles
{
    class Program
    {
        static void Main(string[] args)
        {
            string fajlnev = "teszt.xml";

            //olvasas

            XmlReader olvaso = XmlReader.Create(fajlnev);

            while (olvaso.Read())
            {
                switch (olvaso.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        Console.WriteLine("<{0}>", olvaso.Name);
                        break;

                    case XmlNodeType.Element:
                        Console.WriteLine("<{0}>", olvaso.Name);

                        if (olvaso.HasAttributes)
                        {
                            for (int i = 0; i < olvaso.AttributeCount; ++i)
                            {
                                olvaso.MoveToAttribute(i);
                                //olvaso.MoveToNextAttribute();
                                Console.WriteLine("attributum neve:{0} erteke:{1}", olvaso.Name, olvaso.Value);
                            }

                            olvaso.MoveToElement();
                        }

                        break;

                    case XmlNodeType.EndElement:
                        Console.WriteLine("</{0}>", olvaso.Name);
                        break;

                    case XmlNodeType.Text:
                        Console.WriteLine(olvaso.Value);
                        break;

                    default:
                        break;
                };
            }

            olvaso.Close();

            //attributum-olvasas

            using(olvaso = XmlReader.Create(fajlnev))
            {
                IXmlLineInfo xmlSorInfo = (IXmlLineInfo)olvaso;

                //adatot tartalmazo csomopontra ugrik, erteke a Value tulajdonsag
                olvaso.MoveToContent();//<list>
                Console.WriteLine(olvaso.Name);//list
                Console.WriteLine(olvaso.Value);//nincs erteke

                while (olvaso.Read())
                {
                    //átugorja a csomópont gyerekeit
                    //olvaso.Skip();

                    if (olvaso.HasAttributes)
                    {
                        //olvaso.MoveToFirstAttribute();

                        for (int i = 0; i < olvaso.AttributeCount; ++i)
                        {
                            //MoveTo metodusok, az adott helyre pozicionalnak
                            //bool letezikE = olvaso.MoveToAttribute("value");
                            olvaso.MoveToAttribute(i);
                            //olvaso.MoveToNextAttribute();

                            Console.Write(xmlSorInfo.LineNumber + ". sor, ");
                            Console.Write(xmlSorInfo.LinePosition + ". oszlop: ");
                            Console.WriteLine("{0} {1}", olvaso.Name, olvaso.Value);
                        }

                        olvaso.MoveToElement();
                    }
                }
            }

            //iras

            XmlTextWriter iro = new XmlTextWriter("ujTest.xml", Encoding.UTF8);
            iro.Formatting = Formatting.Indented;

            iro.WriteStartDocument();
            iro.WriteComment(DateTime.Now.ToString());
            iro.WriteWhitespace("\n");

            iro.WriteStartElement("Szemelyek");
            iro.WriteAttributeString("Attributum", "Attributum értéke");

            iro.WriteStartElement("Szemely");
            iro.WriteElementString("Név", "Molár Máté");
            iro.WriteElementString("Kor", "42");
            iro.WriteEndElement();

            iro.WriteEndElement();
            iro.WriteEndDocument();

            iro.Close();

            //stringXML 
            string xmlSztring = @"<?xml version=""1.0"" encoding=""utf-16""?>
                            <List>
                                <Employee>
                                    <ID>1</ID>
                                    <First>David</First>
                                    <Last>Smith</Last>
                                    <Salary>10000</Salary>
                                </Employee>
                                <Employee>
                                    <ID>3</ID>
                                    <First>Mark</First>
                                    <Last>Drinkwater</Last>
                                    <Salary>30000</Salary>
                                </Employee>
                                <Employee>
                                    <ID>4</ID>
                                    <First>Norah</First>
                                    <Last>Miller</Last>
                                    <Salary>20000</Salary>
                                </Employee>
                                <Employee>
                                    <ID>12</ID>
                                    <First>Cecil</First>
                                    <Last>Walker</Last>
                                    <Salary>120000</Salary>
                                </Employee>
                            </List>";

            using (StringReader stringolvaso = new StringReader(xmlSztring))
                using (XmlTextReader xmlOlvaso = new XmlTextReader(stringolvaso))
                {
                    while (xmlOlvaso.Read())
                    {
                        //kezdoelemre ugrik
                        if (xmlOlvaso.IsStartElement())
                        {
                            switch (xmlOlvaso.Name)
                            {
                                case "Employee":
                                    Console.WriteLine();
                                    break;
                                case "ID":
                                    Console.WriteLine("ID: " + xmlOlvaso.ReadString());
                                    break;
                                case "First":
                                    Console.WriteLine("First: " + xmlOlvaso.ReadString());
                                    break;
                                case "Last":
                                    Console.WriteLine("Last: " + xmlOlvaso.ReadString());
                                    break;
                                case "Salary":
                                    Console.WriteLine("Salary: " + xmlOlvaso.ReadString());
                                    break;
                            }
                        }
                    }
                }

        }
    }
}
