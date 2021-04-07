using System;
using System.Xml;

namespace Xml_adatkezeles_DOM
{
    class Program
    {
        static void Main(string[] args)
        {
            //XmlNode -> XmlDocument
            XmlDocument xmlDokumentum = new XmlDocument();
            
            //stream-bol olvas
            xmlDokumentum.Load("teszt.xml");
            
            //sztring-bol olvas
            //xmlDokumentum.LoadXml(@"<?xml version=""1.0"" encoding=""utf - 8"" ?>
            //            <list>
            //                <item>1</item>
            //                <item>2</item>
            //                <item>3</item>
            //            </list>");

            foreach (XmlNode csomopont in xmlDokumentum.ChildNodes)
            {
                //HasChildNodes property is van, leteznek-e gyerekek
                Console.WriteLine(csomopont.Name);
            }

            xmlDokumentum = new XmlDocument();

            //XmlDocument esemenyeket is szolgaltat
            XmlNodeChangedEventHandler lekezelo = null;

            lekezelo = (sender, e) =>
            {
                Console.WriteLine("Node.Name: " + e.Node.Name);
                Console.WriteLine("Node.Value: " + e.Node.Value);
            };

            xmlDokumentum.NodeInserting += lekezelo;

            //XmlNode -> XmlElement, XmlText
            XmlElement element = xmlDokumentum.CreateElement("Elem");
            XmlText text = xmlDokumentum.CreateTextNode("szoveg elemben");
            
            XmlNode node = xmlDokumentum.AppendChild(element);
            node.AppendChild(text);
            
            xmlDokumentum.Save("domteszt.xml");//<Elem>szoveg elemben</Elem>

            //csucs klonozasa
            XmlNode forras = xmlDokumentum.CreateNode(XmlNodeType.Element, "teszt", "nevterURI_teszt");
            //CloneNode(bool deep) - gyerekeket is masoljuk-e
            XmlNode destination = forras.CloneNode(false);
            //van RemoveChild, ReplaceChild is

        }
    }
}
