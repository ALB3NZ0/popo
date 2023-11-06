using System;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace TextEditor
{
    class Figure
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Figure(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }
    }

    class FileEditor
    {
        private string filepath;

        public FileEditor(string filepath)
        {
            this.filepath = filepath;
        }

        public void Load()
        {
            string extension = Path.GetExtension(filepath).ToLower();
            if (extension == ".txt")
            {
                LoadTxt();
            }
            else if (extension == ".json")
            {
                LoadJson();
            }
            else if (extension == ".xml")
            {
                LoadXml();
            }
            else
            {
                throw new Exception("Неподдерживаемый формат файла");
            }
        }

        public void Save(Figure figure)
        {
            string extension = Path.GetExtension(filepath).ToLower();
            if (extension == ".txt")
            {
                SaveTxt(figure);
            }
            else if (extension == ".json")
            {
                SaveJson(figure);
            }
            else if (extension == ".xml")
            {
                SaveXml(figure);
            }
            else
            {
                throw new Exception("Неподдерживаемый формат файла");
            }
        }

        private void LoadTxt()
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        private void LoadJson()
        {
            string json = File.ReadAllText(filepath);
            Figure figure = JsonSerializer.Deserialize<Figure>(json);

            Console.WriteLine($"Name: {figure.Name}");
            Console.WriteLine($"Width: {figure.Width}");
            Console.WriteLine($"Height: {figure.Height}");
        }

        private void LoadXml()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filepath);

            XmlSerializer serializer = new XmlSerializer(typeof(Figure));
            using (XmlReader reader = new XmlNodeReader(xmlDocument.DocumentElement))
            {
                Figure figure = (Figure)serializer.Deserialize(reader);

                Console.WriteLine($"Name: {figure.Name}");
                Console.WriteLine($"Width: {figure.Width}");
                Console.WriteLine($"Height: {figure.Height}");
            }
        }

        private void SaveTxt(Figure figure)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine($"Name: {figure.Name}");
                sw.WriteLine($"Width: {figure.Width}");
                sw.WriteLine($"Height: {figure.Height}");
            }
        }

        private void SaveJson(Figure figure)
        {
            string json = JsonSerializer.Serialize(figure);
            File.WriteAllText(filepath, json);
        }

        private void SaveXml(Figure figure)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Figure));
            using (XmlWriter writer = XmlWriter.Create(filepath))
            {
                serializer.Serialize(writer, figure);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу:");
            string filepath = Console.ReadLine();

            FileEditor fileEditor = new FileEditor(filepath);

            try
            {
                fileEditor.Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}