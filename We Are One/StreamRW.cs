using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace We_Are_One
{
    internal class StreamRW
    {
        private string file = "";

        private string filefav = "bin/fav.cfg";

        public StreamRW(string file)
        {
            this.file = file;
        }

        public string GetUrl(string streamname)
        {
            return "";
        }

        public string GetStreamname(string url)
        {
            return "";
        }

        public string[] GetStreamList()
        {
            string text = string.Empty;
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(this.file))
            {
                while ((str = streamReader.ReadLine()) != null)
                {
                    text += str;
                }
            }
            return text.Split(new char[]
            {
                ';'
            });
        }

        public string[] GetFavList()
        {
            string text = string.Empty;
            string str = string.Empty;
            using (StreamReader streamReader = new StreamReader(this.filefav))
            {
                while ((str = streamReader.ReadLine()) != null)
                {
                    text += str;
                }
            }
            return text.Split(new char[]
            {
                ';'
            });
        }

        public bool ContainsStreamInFile(string streamname, string url)
        {
            string text = string.Empty;
            bool result;
            using (StreamReader streamReader = new StreamReader(this.file))
            {
                while ((text = streamReader.ReadLine()) != null)
                {
                    if (text.Contains(streamname) || text.Contains(url))
                    {
                        result = true;
                        return result;
                    }
                }
            }
            result = false;
            return result;
        }

        public bool AddStream(string streamname, string url)
        {
            bool result;
            if (this.ContainsStreamInFile(streamname, url))
            {
                result = false;
            }
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(this.file, true))
                {
                    streamWriter.WriteLine(streamname + ";" + url + ";");
                }
                result = true;
            }
            return result;
        }

        public bool AddFav(string streamname, string url)
        {
            using (StreamWriter streamWriter = new StreamWriter(this.filefav, true))
            {
                streamWriter.WriteLine(streamname + ";" + url + ";");
            }
            return true;
        }

        public bool DeleteStream(string streamname, string url)
        {
            string text = string.Empty;
            string text2 = string.Empty;
            using (StreamReader streamReader = new StreamReader(this.file))
            {
                while ((text2 = streamReader.ReadLine()) != null)
                {
                    if (!text2.Contains(streamname) || !text2.Contains(url))
                    {
                        text += text2;
                    }
                }
            }
            string[] array = text.Split(new char[]
            {
                ';'
            });
            File.Delete(this.file);
            for (int i = 0; i <= array.Length - 2; i += 2)
            {
                using (StreamWriter streamWriter = new StreamWriter(this.file, true))
                {
                    streamWriter.WriteLine(array[i] + ";" + array[i + 1] + ";");
                }
            }
            return true;
        }

        public bool DeleteFav(string streamname, string url)
        {
            string text = string.Empty;
            string text2 = string.Empty;
            using (StreamReader streamReader = new StreamReader(this.filefav))
            {
                while ((text2 = streamReader.ReadLine()) != null)
                {
                    if (!text2.Contains(streamname) || !text2.Contains(url))
                    {
                        text += text2;
                    }
                }
            }
            string[] array = text.Split(new char[]
            {
                ';'
            });
            File.Delete(this.filefav);
            for (int i = 0; i <= array.Length - 2; i += 2)
            {
                using (StreamWriter streamWriter = new StreamWriter(this.filefav, true))
                {
                    streamWriter.WriteLine(array[i] + ";" + array[i + 1] + ";");
                }
            }
            return true;
        }
    }
}
