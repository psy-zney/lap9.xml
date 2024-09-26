using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace xml
{
   

    [Serializable]
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
    }

    [Serializable]
    public class StudentList
    {
        public List<Student> Students { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
           Console.OutputEncoding   = Encoding.UTF8;
            // Tạo danh sách sinh viên ban đầu
            var studentList = new StudentList
            {
                Students = new List<Student>
            {
                new Student { id = 1, name = "Khanh", age = 17 },
                new Student { id = 2,name= "Linh",age= 17 }
            }
            };

            // Serialize thành XML và ghi vào file
            using (FileStream fs = new FileStream("data.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StudentList));
                serializer.Serialize(fs, studentList);
            }

            // Đọc dữ liệu từ file
            using (FileStream fs = new FileStream("data.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StudentList));

                // Tạo một XmlReader để xử lý các lỗi liên quan đến XML
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    try
                    {
                        StudentList deserializedStudentList = (StudentList)serializer.Deserialize(reader);
                        // Thêm sinh viên mới
                        deserializedStudentList.Students.Add(new Student { id = 3, name = "Anh", age = 18 });

                        // Ghi đè lên file cũ
                        fs.Seek(0, SeekOrigin.Begin); // Đặt con trỏ về đầu file
                        serializer.Serialize(fs, deserializedStudentList);

                    }
                    catch (XmlException ex)
                    {
                        Console.WriteLine("Lỗi khi đọc file XML: " + ex.Message);
                        // Thêm các xử lý lỗi khác ở đây, ví dụ: ghi log, thông báo cho người dùng
                    }
                }
                
            }
            // Đọc lại và hiển thị dữ liệu
            using (FileStream fs = new FileStream("data.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(StudentList));
                StudentList finalStudentList = (StudentList)serializer.Deserialize(fs);

                Console.WriteLine("Danh sách sinh viên sau khi cập nhật:");
                foreach (var student in finalStudentList.Students)
                {
                    Console.WriteLine($"ID: {student.id}, Tên: {student.name}, Tuổi: {student.age}");
                }
            }
            Console.ReadLine();
        }
    }
}
    

