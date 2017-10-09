using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace TheseThree.Admin.Utils
{
    public class IoHelper 
    {
        public static void CreateDir(string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        public static string CreateFile(string path,string path2, string content)
        {
            var fileName = DateTime.Now.Ticks + ".html";
            using (var fs = new FileStream(path+fileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs,Encoding.Default))
                {
                    sw.WriteLine(content);
                }
            }
            using (var fs = new FileStream(path2 + fileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine(content);
                }
            }
            return fileName;
        }
    }
}