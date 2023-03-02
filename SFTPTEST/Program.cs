using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SFTPTEST
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var localPath = @"D:\";
            var keyFile = new PrivateKeyFile(@"D:\TOOLS\TOOL_SFTP金鑰檔案\fannie.pem");
            var keyFiles = new[] { keyFile };
            var username = "fannie";

            var methods = new List<AuthenticationMethod>();
            //methods.Add(new PasswordAuthenticationMethod(username, "password"));
            methods.Add(new PrivateKeyAuthenticationMethod(username, keyFiles));

            var con = new ConnectionInfo("127.0.0.1", 22, username, methods.ToArray());
            using (var client = new SftpClient(con))
            {
                client.Connect();
                Console.WriteLine("已連線");
                var files = client.ListDirectory("/目的資料夾/");
                foreach (var file in files)
                {
                    Console.WriteLine(file);

                    using (var fs = new FileStream(localPath + file.Name, FileMode.Create))
                    {
                        client.DownloadFile(file.FullName, fs);
                    }
                }
                Console.WriteLine("下載成功");
                client.ChangeDirectory("/目的資料夾/"); //更換工作資料夾
                var sourcefile = "rileytest.txt";
                using (FileStream fs0 = new FileStream(localPath+ sourcefile, FileMode.Open, FileAccess.Read))
                {
                    //client.BufferSize = 4 * 1024;
                    client.UploadFile(fs0, Path.GetFileName(localPath+sourcefile));
                    Console.WriteLine("上傳成功");
                }


            }
        }
    }
}
