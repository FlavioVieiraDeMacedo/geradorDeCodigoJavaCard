using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaCardCreator.BO
{
    class ArquivosBo
    {
        public string path { get; set; }
        public string SourcePath { get; set; }
        public string[] diretorios { get; set; }
        public string[] arquivos { get; set; }
        public DirectoryInfo pacote { get; set; }
        public DirectoryInfo applet { get; set; }
        public string NameCap { get; set; }
        public string AppletID { get; set; }
        public string nomeapplet { get; set; }


        public void CreateFolders()
        {
            diretorios = Directory.GetDirectories(path + @"\src");//pega partas dentro da src
            arquivos = Directory.GetFiles(diretorios[0]);//pega arquivos dentro da partas dentro da src
            pacote = new DirectoryInfo(diretorios[0]);
            applet = new DirectoryInfo(arquivos[0]);
            nomeapplet = Path.GetFileNameWithoutExtension(arquivos[0]);
            Directory.CreateDirectory(path + "/docx");
            Directory.CreateDirectory(path + "/_install");
            Directory.CreateDirectory(path + "/classes");
            Directory.CreateDirectory(path + "/build");
            Directory.CreateDirectory(path + "/opt");
            Directory.CreateDirectory(path + "/exp");
            Directory.CreateDirectory(path + "/JCS");
            Directory.CreateDirectory(path + "/lib");
            Directory.CreateDirectory(path + "/profile");
        }

        public void Copyfolders() {
            string a = SourcePath + @"\exp";
            DirectoryCopy(a, path + @"\exp", true);
            a = SourcePath + @"\JCS";
            DirectoryCopy(a, path + @"\JCS", true);
            a = SourcePath + @"\lib";
            DirectoryCopy(a, path + @"\lib", true);


            string fileName = "setEnv.bat";
            string sourceFile = System.IO.Path.Combine(SourcePath, fileName);
            string destFile = System.IO.Path.Combine(path, fileName);
            File.Copy(sourceFile, destFile, true);
        }

        
        public void createCompile()
        {
            using (StreamWriter sw = File.CreateText(path + @"\compile.bat"))
            {
                sw.WriteLine("@echo off");
                sw.WriteLine("");
                sw.WriteLine("call setEnv");
                sw.WriteLine("");
                sw.WriteLine(@"javac -target %TARGET% -g -classpath %CLASSPATH%;%SRC_DIR% -d %CLASS_DIR% " + arquivos[0]);
                sw.WriteLine("");
                sw.WriteLine("pause");
            }
        }
       

       
        public void CriateConvert()
        {
            string nomeapplet = Path.GetFileNameWithoutExtension(arquivos[0]);
            using (StreamWriter sw = File.CreateText(path + @"\convert.bat"))
            {

                sw.WriteLine("@echo off");
                sw.WriteLine("");
                sw.WriteLine("call setEnv");
                sw.WriteLine("");
                sw.WriteLine("echo 'PRODUCTION VERSION'");
                sw.WriteLine(@"java -classpath %CONVERTER% com.sun.javacard.converter.Converter -config .\opt\prod.opt");
                if (NameCap==null||NameCap=="")
                {
                    sw.WriteLine(@"copy .\build\" + pacote.Name + @"\javacard\" + pacote.Name + @".cap .\_install\" + nomeapplet + ".cap");
                }
                else
                {
                    sw.WriteLine(@"copy .\build\" + pacote.Name + @"\javacard\" + pacote.Name + @".cap .\_install\" + NameCap + ".cap");
                }

                sw.WriteLine("");
                sw.WriteLine("echo 'DEBUG VERSION'");
                sw.WriteLine(@"java -classpath %CONVERTER% com.sun.javacard.converter.Converter -config .\opt\debug.opt");
                if (NameCap == null || NameCap == "")
                {
                    sw.WriteLine(@"copy .\build\" + pacote.Name + @"\javacard\" + pacote.Name + @".cap .\profile\" + nomeapplet + ".cap");
                }
                else
                {
                    sw.WriteLine(@"copy .\build\" + pacote.Name + @"\javacard\" + pacote.Name + @".cap .\profile\" + NameCap + ".cap");
                }
                sw.WriteLine("");
                sw.WriteLine("pause");
            }
        }
      


        
        public void CriateProd()
        {
            using (StreamWriter sw = File.CreateText(path + @"\opt\prod.opt"))
            {

                sw.WriteLine("-v");
                sw.WriteLine(@"-classdir .\classes");
                sw.WriteLine("-noverify");
                sw.WriteLine(@"-exportpath .\exp\");
                sw.WriteLine(@"-d .\build\");
                sw.WriteLine("-out EXP CAP JCA");
                sw.WriteLine("-applet");
                if (AppletID == null || AppletID == "")
                {
                    sw.WriteLine("0xD2:0x76:0x00:0x01:0x18:0x00:0x02:0x00:0x55:0x06:0x10:0x89:0x46:0x43:0x4C:0x01");
                }
                else
                {
                    string aux = null;
                    for (int i = 0; i < 32; i++)
                    {
                        if (i == 30)
                        {
                            aux += "0x" + AppletID.Substring(i, 2);
                        }
                        else
                        {
                            aux += "0x" + AppletID.Substring(i, 2) + ":";
                        }
                        i++;
                    }
                    sw.WriteLine(aux);
                }


                sw.WriteLine(pacote.Name + "." + nomeapplet);
                sw.WriteLine(pacote.Name);
                if (AppletID == null || AppletID == "")
                {
                    sw.WriteLine("0xD2:0x76:0x00:0x01:0x18:0x00:0x02:0x00:0x55:0x06:0x10:0x89:0x46:0x43:0x4C:0x00 1.1");
                }
                else
                {
                    string aux = null;
                    for (int i = 0; i < 32; i++)
                    {
                        if (i == 30)
                        {
                            aux += "0x00 1.1";
                        }
                        else
                        {
                            aux += "0x" + AppletID.Substring(i, 2) + ":";
                        }
                        i++;
                    }
                    sw.WriteLine(aux);
                }
            }
        }
        

       
        public void CriateDebug()
        {
            using (StreamWriter sw = File.CreateText(path + @"\opt\debug.opt"))
            {

                sw.WriteLine("-v");
                sw.WriteLine(@"-classdir .\classes");
                sw.WriteLine("-noverify");
                sw.WriteLine(@"-exportpath .\exp\");
                sw.WriteLine(@"-d .\build\");
                sw.WriteLine("-debug");
                sw.WriteLine("-out EXP CAP JCA");
                sw.WriteLine("-applet");
                if (AppletID == null || AppletID == "")
                {
                    sw.WriteLine("0xD2:0x76:0x00:0x01:0x18:0x00:0x02:0x00:0x55:0x06:0x10:0x89:0x46:0x43:0x4C:0x01");
                }
                else
                {
                    string aux = null;
                    for (int i = 0; i < 32; i++)
                    {
                        if (i == 30)
                        {
                            aux += "0x" + AppletID.Substring(i, 2);
                        }
                        else
                        {
                            aux += "0x" + AppletID.Substring(i, 2) + ":";
                        }
                        i++;
                    }
                    sw.WriteLine(aux);
                }


                sw.WriteLine(pacote.Name + "." + nomeapplet);
                sw.WriteLine(pacote.Name);
                if (AppletID == null || AppletID == "")
                {
                    sw.WriteLine("0xD2:0x76:0x00:0x01:0x18:0x00:0x02:0x00:0x55:0x06:0x10:0x89:0x46:0x43:0x4C:0x00 1.1");
                }
                else
                {
                    string aux = null;
                    for (int i = 0; i < 32; i++)
                    {
                        if (i == 30)
                        {
                            aux += "0x00 1.1";
                        }
                        else
                        {
                            aux += "0x" + AppletID.Substring(i, 2) + ":";
                        }
                        i++;
                    }
                    sw.WriteLine(aux);
                }

            }
        }
            
            

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                // Create the path to the new copy of the file.
                var temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, true);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (!copySubDirs) return;

            foreach (var subdir in dirs)
            {
                // Create the subdirectory.
                var temppath = Path.Combine(destDirName, subdir.Name);

                // Copy the subdirectories.
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }

        public static string HEX2ASCII(string hex)
        {
            string res = String.Empty;
            try
            {
                for (int a = 0; a < hex.Length; a = a + 2)
                {
                    string Char2Convert = hex.Substring(a, 2);
                    int n = Convert.ToInt32(Char2Convert, 16);
                    char c = (char)n;
                    res += c.ToString();
                }
                return res;
            }
            catch (Exception)
            {
                return res;
            }
        }

        public static string ASCIITOHex(string ascii)
        {
            StringBuilder sb = new StringBuilder();
            byte[] inputBytes = Encoding.UTF8.GetBytes(ascii);
            foreach (byte b in inputBytes)
            {
                sb.Append(string.Format("{0:x2}", b));
            }
            return sb.ToString();
        }

    }
}
