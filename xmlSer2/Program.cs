#define ATTRIBUTUM

using System;
using System.Collections.Generic; //List<>
using System.Linq; //linq
using System.Xml.Serialization; //XmlSerializer, XmlType, XmlAttribute, XmlElement
using System.IO; //StreamReader, StreamWriter
using System.Xml.Linq; //XElement

namespace xmlSer2
{
    public enum Sex { female, male }

    public class CourseForStudent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }        
        [XmlIgnore]
        public int Credits { get; set; }
    }

#if ATTRIBUTUM
    [XmlType("StudentWithCourses")]
#endif
    public class Student
    {
#if ATTRIBUTUM
        [XmlAttribute]
#endif
        public string FirstName { get; set; }

#if ATTRIBUTUM
        [XmlAttribute("last_name")]
#endif
        public string LastName { get; set; }

#if ATTRIBUTUM
        [XmlAttribute("Gender")]
#endif
        public Sex Sex { get; set; }

#if ATTRIBUTUM
        [XmlElement("BirthDate")]
#endif
        public DateTime DateOfBirth { get; set; }

        public List<CourseForStudent> Courses { set; get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            XElement xStudents = XElement.Load("students.xml");
            XElement xCourses = XElement.Load("courses.xml");

            string firstName = xStudents.Element("student").Attribute("firstName").Value;

            //ertekek beolvasasa is string lesz
            string year = xStudents.Elements("student")
                                    .ElementAt(2)
                                    .Element("dateOfBirth").Attribute("year").Value;

            var femaleStudents = from s in xStudents.Elements("student")
                                 where s.Attribute("sex").Value == "female"
                                 select new
                                 {
                                     FirstName = s.Attribute("firstName").Value,
                                     LastName = s.Attribute("lastName").Value,
                                     //string olvasasa miatt datumma kell alakitani
                                     DateOfBirth = new DateTime(
                                                 int.Parse(s.Element("dateOfBirth").Attribute("year").Value),
                                                 int.Parse(s.Element("dateOfBirth").Attribute("month").Value),
                                                 int.Parse(s.Element("dateOfBirth").Attribute("day").Value)
                                         )
                                 };

            var studentsWithCourses = (from s in xStudents.Elements("student")
                                       select new Student
                                       {
                                           FirstName = s.Attribute("firstName").Value,
                                           LastName = s.Attribute("lastName").Value,
                                           Sex = (Sex)Enum.Parse(typeof(Sex), s.Attribute("sex").Value),
                                           DateOfBirth = new DateTime(
                                                  int.Parse(s.Element("dateOfBirth").Attribute("year").Value),
                                                  int.Parse(s.Element("dateOfBirth").Attribute("month").Value),
                                                  int.Parse(s.Element("dateOfBirth").Attribute("day").Value)
                                          ),

                                           Courses = (from c1 in s.Element("courses").Elements("course")
                                                      join c2 in xCourses.Elements("course")
                                                      on c1.Attribute("id").Value equals c2.Attribute("id").Value
                                                      select new CourseForStudent
                                                      {
                                                          Id = c1.Attribute("id").Value,
                                                          Name = c2.Attribute("name").Value,
                                                          Grade = int.Parse(c1.Attribute("grade").Value),
                                                          Credits = int.Parse(c2.Attribute("credits").Value)
                                                      }).ToList()
                                       }).ToList();

            string filename = "query.xml";
#if ATTRIBUTUM
            //<AllStudents xmlns
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>), new XmlRootAttribute("AllStudents"));
#else
            //<ArrayOfStudent xmlns
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
#endif
            StreamWriter fileWriter = new StreamWriter(filename);
            serializer.Serialize(fileWriter, studentsWithCourses);
            fileWriter.Close();


            //XmlSerializer serializer = new XmlSerializer(typeof(List<Student>), new XmlRootAttribute("AllStudents"));
            StreamReader fileReader = new StreamReader(filename);
            var students = serializer.Deserialize(fileReader) as List<Student>;
            fileReader.Close();

            foreach (var student in students)
            {
                Console.WriteLine($"{student.FirstName} {student.LastName}, {student.Sex}, courses: {student.Courses.Count}");
            }
        }
    }
}
