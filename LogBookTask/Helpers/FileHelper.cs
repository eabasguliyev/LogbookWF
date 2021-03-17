using System;
using System.IO;
using System.Text;
using LogBookTask.Entities;
using Newtonsoft.Json;

namespace LogBookTask.Helpers
{
    public static class FileHelper
    {
        private static JsonSerializer Serializer;

        static FileHelper()
        {
            Serializer = new JsonSerializer();
        }

        public static void WriteToJson(string fileName, Lesson lesson)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;

                        Serializer.Serialize(jw, lesson);
                    }
                }
            }
        }

        public static void WriteClassToJson(string fileName, Class classObj)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = Formatting.Indented;

                        Serializer.Serialize(jw, classObj);
                    }
                }
            }
        }

        public static Lesson ReadFromJson(string fileName)
        {
            Lesson lesson = null;
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    using (var jr = new JsonTextReader(sr))
                    {
                        lesson = Serializer.Deserialize<Lesson>(jr);
                    }
                }
            }

            return lesson;
        }

        public static Class ReadClassFromJson(string fileName)
        {
            Class classObj = null;
            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    using (var jr = new JsonTextReader(sr))
                    {
                        classObj = Serializer.Deserialize<Class>(jr);
                    }
                }
            }

            return classObj;
        }
    }
}