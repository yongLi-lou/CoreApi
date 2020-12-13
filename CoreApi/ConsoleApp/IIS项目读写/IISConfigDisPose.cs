using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConsoleApp
{
    public class IISConfigDisPose
    {
        /// <summary>
        /// 写入IIs项目
        /// </summary>
        public void SetIISConfig(string jsonfile)
        {

            StreamReader sr = new StreamReader(jsonfile, Encoding.UTF8);
            String line;
            string jsonText = "";
            while ((line = sr.ReadLine()) != null)
            {
                jsonText += line.ToString();
            }

            var jsonlist = JsonConvert.DeserializeObject<List<ProjectModel>>(jsonText);




            //var listPj = new List<Tuple<string, string, string, string>>();
            //listPj.Add(new Tuple<string, string, string, string>("名称3", "普通", "地址", "8992"));
            //listPj.Add(new Tuple<string, string, string, string>("名称2", "Core", "地址", "8993"));



            //配置文件地址
            string configpath = @"C:\Windows\System32\inetsrv\Config\applicationHost.config";
            //备份文件名称
            string configpathCopy = $@"C:\Windows\System32\inetsrv\Config\applicationHost_备份{DateTime.Now.ToString("yyyyMMddhhmmss")}.config";
            bool isrewrite = true;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configpath);
            XmlNode configuration = xmlDoc.GetElementsByTagName("system.applicationHost").Item(0);
            //XmlNodeList ndlist2 = xmlDoc.SelectNodes("//xsl:attribute[@name=’src’]");
            //程序池
            XmlNode applicationPools = configuration.SelectNodes("//applicationPools").Item(0);
            //项目
            XmlNode sites = configuration.SelectNodes("//sites").Item(0);





            bool bol = true;
            foreach (var item in jsonlist)
            {
                var aname = item.projectname;
                var prot = item.projectport;

                //判断程序池
                var xmlcount = applicationPools.SelectNodes($"//add[@name='{aname}']").Count;
                //判断项目
                var pjcount = sites.SelectNodes($"//add[@name='{aname}']").Count;
                //判断端口
                var protcount = sites.SelectNodes($"//binding[@bindingInformation='*:{prot}:']").Count;

                if (xmlcount > 0)
                {
                    Shell.WriteLineRed($"\r\n 已存在程序池 【{aname}】");
                    bol = false;
                }
                else if (pjcount > 0)
                {
                    Shell.WriteLineRed($"\r\n 已存在项目名称 【{aname}】");
                    bol = false;
                }
                else if (protcount > 0)
                {
                    Shell.WriteLineRed($"\r\n 已存在端口 【{prot}】");
                    bol = false;
                }
            }

            if (bol)
            {
                foreach (var item in jsonlist)
                {
                    var aname = item.projectname;
                    var atype = item.pooltype;
                    var address = item.projectaddress;
                    var prot = item.projectport;

                    #region 添加程序池

                    XmlNode aftnode = applicationPools.LastChild;
                    XmlNode newnode = xmlDoc.CreateElement("add");
                    XmlAttribute newattribute = xmlDoc.CreateAttribute("name");
                    newattribute.Value = aname;
                    newnode.Attributes.Append((XmlAttribute)newattribute);
                    if (atype.ToUpper() == "CORE")
                    {
                        newattribute = xmlDoc.CreateAttribute("managedRuntimeVersion");
                        newattribute.Value = "";
                        newnode.Attributes.Append((XmlAttribute)newattribute);
                    }
                    applicationPools.InsertBefore(newnode, aftnode);

                    #endregion

                    #region 添加项目

                    XmlNodeList sitlist = sites.SelectNodes("//site");
                    aftnode = sitlist.Item(sitlist.Count - 1);
                    var id = aftnode.Attributes["id"].Value.FormatToInt(0) + 1;

                    newnode = xmlDoc.CreateElement("site");
                    newattribute = xmlDoc.CreateAttribute("name");
                    newattribute.Value = aname;
                    newnode.Attributes.Append((XmlAttribute)newattribute);
                    newattribute = xmlDoc.CreateAttribute("id");
                    newattribute.Value = id.ToString();
                    newnode.Attributes.Append((XmlAttribute)newattribute);
                    sites.InsertAfter(newnode, aftnode);

                    //添加x项目程序池
                    var newnodecc = xmlDoc.CreateElement("application");
                    newattribute = xmlDoc.CreateAttribute("path");
                    newattribute.Value = "/";
                    newnodecc.Attributes.Append((XmlAttribute)newattribute);
                    newattribute = xmlDoc.CreateAttribute("applicationPool");
                    newattribute.Value = aname;
                    newnodecc.Attributes.Append((XmlAttribute)newattribute);
                    newnode.AppendChild(newnodecc);

                    //添加程序池下项目地址
                    var newnodecc2 = xmlDoc.CreateElement("virtualDirectory");
                    newattribute = xmlDoc.CreateAttribute("path");
                    newattribute.Value = "/";
                    newnodecc2.Attributes.Append((XmlAttribute)newattribute);
                    newattribute = xmlDoc.CreateAttribute("physicalPath");
                    newattribute.Value = address;
                    newnodecc2.Attributes.Append((XmlAttribute)newattribute);
                    newnodecc.AppendChild(newnodecc2);


                    //添加x绑定端口bind
                    var newnodebind = xmlDoc.CreateElement("bindings");
                    newnode.AppendChild(newnodebind);

                    //添加x绑定端口
                    var newnodebind2 = xmlDoc.CreateElement("binding");
                    newattribute = xmlDoc.CreateAttribute("protocol");
                    newattribute.Value = "http";
                    newnodebind2.Attributes.Append((XmlAttribute)newattribute);
                    newattribute = xmlDoc.CreateAttribute("bindingInformation");
                    newattribute.Value = $"*:{prot}:";
                    newnodebind2.Attributes.Append((XmlAttribute)newattribute);
                    newnodebind.AppendChild(newnodebind2);
                    #endregion

                }

                //先拷贝备份
                System.IO.File.Copy(configpath, configpathCopy, isrewrite);

                xmlDoc.Save(configpath);
            }

        }


        /// <summary>
        /// 读取IIs项目
        /// </summary>
        public string GetIISConfig()
        {

            var listpools = new List<ApplicationPoolModel>();
            var listproject = new List<ProjectModel>();

            //配置文件地址
            string configpath = @"C:\Windows\System32\inetsrv\Config\applicationHost.config";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configpath);
            XmlNode configuration = xmlDoc.GetElementsByTagName("system.applicationHost").Item(0);
            //XmlNodeList ndlist2 = xmlDoc.SelectNodes("//xsl:attribute[@name=’src’]");
            //程序池
            XmlNode applicationPools = configuration.SelectNodes("//applicationPools").Item(0);
            var listnode = applicationPools.SelectNodes($"//applicationPools/add");
            //程序池
            foreach (XmlNode item in listnode)
            {
                var poolname = item.Attributes["name"].Value;
                var pooltype = "普通";
                if (item.Attributes["managedRuntimeVersion"] != null)
                {
                    if (item.Attributes["managedRuntimeVersion"].Value == "")
                    {
                        pooltype = "CORE";
                    }
                }

                listpools.Add(new ApplicationPoolModel { poolname = poolname, pooltype = pooltype });
            }

            //项目
            XmlNode sites = configuration.SelectNodes("//sites").Item(0);
            listnode = applicationPools.SelectNodes($"//sites/site");
            //程序池
            foreach (XmlNode item in listnode)
            {
                //项目名称
                var projectname = item.Attributes["name"].Value;
                var poolname = "";
                var projectaddress = "";
                var projectport = "";

                //程序池名称
                var poolapplicate = item.SelectNodes($"//site[@name='{projectname}']/application");
                if (poolapplicate.Count > 0)
                {
                    var poolmodel = poolapplicate.Item(0).Attributes["applicationPool"];
                    if (poolmodel != null)
                    {
                        poolname = poolmodel.Value;
                    }
                }

                //项目地址
                var virtualDirectory = item.SelectNodes($"//site[@name='{projectname}']/application/virtualDirectory");
                if (virtualDirectory.Count > 0)
                {
                    var physicalPath = virtualDirectory.Item(0).Attributes["physicalPath"];
                    if (physicalPath != null)
                    {
                        projectaddress = physicalPath.Value;
                    }
                }

                //端口
                var binding = item.SelectNodes($"//site[@name='{projectname}']/bindings/binding");
                if (binding.Count > 0)
                {
                    var bindingInformation = binding.Item(0).Attributes["bindingInformation"];
                    if (bindingInformation != null)
                    {
                        projectport = bindingInformation.Value;
                        if (projectport.StartsWith("*:"))
                        {
                            projectport = projectport.Replace("*:", "");
                            projectport = projectport.Replace(":", "");
                        }
                    }
                }


                listproject.Add(new ProjectModel
                {
                    projectname = projectname,
                    poolname = poolname,
                    pooltype = listpools.Where(e => e.poolname == poolname).Count() > 0 ? listpools.Where(e => e.poolname == poolname).FirstOrDefault().pooltype : "普通",
                    projectaddress = projectaddress.Replace(@"\", @"\\"),
                    projectport = projectport,

                });
            }

            var strajson = listproject.ToJson();

            return strajson;
        }


        public void AppStart()
        {
            Shell.WriteLine("-----------------启动IIS一键发布工具----------------");
            Shell.WriteLine("----------------------------------------------------");
            Shell.WriteLine("");
            Shell.WriteLine("命令：read (读取IIS目录)，write（写入IIS目录）");
            Shell.WriteLine("请输入命令：");
            var ml = Console.ReadLine();
            if (ml == "read")
            {


                string jsonstr = new IISConfigDisPose().GetIISConfig();
                Shell.WriteLine("读取成功：");
                Shell.WriteLine("");
                Shell.WriteLine("");
                Shell.WriteLine("");
                Shell.WriteLine(jsonstr);

                string dir = AppDomain.CurrentDomain.BaseDirectory + "/Logs";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                var path = dir + "\\" + "sqlexe.txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                FileStream fs = File.Create(path);
                fs.Close();

                StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default);
                sw.WriteLine("--------------------Start--------------------");
                sw.WriteLine(jsonstr);
                sw.WriteLine("--------------------End--------------------");
                sw.Close();

            }
            else if (ml == "write")
            {
                Console.WriteLine("请输入地址：");
                var jsonfile = Console.ReadLine();

                new IISConfigDisPose().SetIISConfig(jsonfile);
                Shell.WriteLine("写入完毕，请刷新IIS：");


            }


            Console.ReadKey(); Shell.WriteLine("-----------------启动IIS一键发布工具----------------");
            Shell.WriteLine("----------------------------------------------------");
            Shell.WriteLine("");
            Shell.WriteLine("命令：read (读取IIS目录)，write（写入IIS目录）");
            Shell.WriteLine("请输入命令：");
            ml = Console.ReadLine();
            if (ml == "read")
            {


                string jsonstr = new IISConfigDisPose().GetIISConfig();
                Shell.WriteLine("读取成功：");
                Shell.WriteLine("");
                Shell.WriteLine("");
                Shell.WriteLine("");
                Shell.WriteLine(jsonstr);

            }
            else if (ml == "write")
            {
                Console.WriteLine("请输入地址：");
                var jsonfile = Console.ReadLine();

                new IISConfigDisPose().SetIISConfig(jsonfile);
                Shell.WriteLine("写入完毕，请刷新IIS：");


            }


            Console.ReadKey();
        }

    }

    public class ApplicationPoolModel
    {
        /// <summary>
        /// 程序池名称
        /// </summary>
        public string poolname { get; set; }

        /// <summary>
        /// 程序池类型
        /// </summary>
        public string pooltype { get; set; }
    }


    public class ProjectModel
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string projectname { get; set; }

        /// <summary>
        /// 程序池名称
        /// </summary>
        public string poolname { get; set; }

        /// <summary>
        /// 程序池类型
        /// </summary>
        public string pooltype { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string projectaddress { get; set; }

        /// <summary>
        /// 项目端口
        /// </summary>
        public string projectport { get; set; }
    }


}
