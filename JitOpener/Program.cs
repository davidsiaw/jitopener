using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using BlueBlocksLib.FileAccess;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using SQLiteBrowser;
using BlueBlocksLib.Reports;
using System.Text.RegularExpressions;
using BlueBlocksLib.SetUtils;

namespace JitOpener {

    struct VersionInfo
    {
        public string title;
        public string sourcePath;
        public string destPath;
        public int cultureNum;
        public string encoding;
        public string indexsuffix;
        public bool allowKorean;
        public string COOL_PAGE_TITLE;
        public string SIMPLE_PAGE_TITLE;
        public Dictionary<string, string> trans;
        public int reinforceVersion;
    }

    static class Program
    {
        public static string webpath = @"W:\www\aikadb";

        public static string title = "AIKA SEA Database";
        public static string sourcePath = @"C:\t3fun\AIKAGlobal";
        public static string destPath = "data";
        public static int cultureNum = 8;
        public static string encoding = "euc-KR";
        public static string indexsuffix = "index.html";
        public static bool allowKorean = false;
        public static string COOL_PAGE_TITLE = "Aika DB of Abyss";
        public static string SIMPLE_PAGE_TITLE = "Aika DB of Divine Oracle";
        public static int reinforceVersion = 1;
        public static Dictionary<string, string> trans = new Dictionary<string, string>();

        public static string AddSpacesBeforeCapitals(string str)
        {
            if (Program.allowKorean)
            {
                return str;
            }

            string res = str[0].ToString();
            for (int i = 1; i < str.Length; i++)
            {
                if (str.ToUpper()[i] == str[i])
                {
                    res += " ";
                }
                res += str[i];
            }
            return res;
        }

        class Bone
        {
            [ArraySize(16)]
            public float[] matrix;
        }

        interface Vertex
        {

        }

        class Vertex48 : Vertex
        {
            //if Csize = 48
            //float_3 ---- Vertex XYZ
            //byte_8 ---bone weight ?
            //float_2 ---Bone weighting ?
            //float_3 --- Normals NX XY NX
            //float_2 --- UV coords ( TV * -1) to flip image 
            public float x;
            public float y;
            public float z;

            public float mystery1;
            [ArraySize(12)]
            public byte[] mystery2;

            public float nx;
            public float ny;
            public float nz;

            public float u;
            public float v;
        }

        class Vertex44 : Vertex
        {
            //if Csize = 44
            //float_3 ---- Vertex XYZ
            //byte_8 ---bone weight ?
            //float_1 ---Bone weighting ?
            //float_3 --- Normals NX XY NX
            //float_2 --- UV coords ( TV * -1) to flip image
            public float x;
            public float y;
            public float z;

            public float mystery1;
            [ArraySize(8)]
            public byte[] mystery2;

            public float nx;
            public float ny;
            public float nz;

            public float u;
            public float v; 
        }

        class Vertex40 : Vertex
        {
            
            //if Csize = 40
            //float_3 ---- Vertex XYZ
            //byte_8 ---bone weight ?
            //float_1 --- Normals NX XY NX
            //float_2 --- UV coords ( TV * -1) to flip image 
            public float x;
            public float y;
            public float z;

            public float weight;
            public byte bone1;
            public byte bone2;
            public short padding;

            public float nx;
            public float ny;
            public float nz;

            public float u;
            public float v;

        }

        class Vertex36 : Vertex
        {
            //if Csize = 36
            //float_3 ---- Vertex XYZ
            //byte_4 ---bone weight ?
            //float_1 --- Normals NX XY NX
            //float_2 --- UV coords ( TV * -1) to flip image 
            public float x;
            public float y;
            public float z;

            public byte bone1;
            public byte bone2;
            public short padding;

            public float nx;
            public float ny;
            public float nz;

            public float u;
            public float v;

            public override string ToString()
            {
                return " b1:" + bone1 + " b2:" + bone2;
            }
        }

        class Face
        {
            [ArraySize(3)]
            public short[] face;
        }

        class MSHFormat
        {
            [ArraySize(4)]
            public byte[] magic;
            [ArraySize(4)]
            public byte[] nothing;
            public int mystery1;
            public int mystery2;
            public int vertex_chunk_length;
            public int mystery3;
            public int bone_array_size;
            public int vertex_array_size;
            public int face_index_count;

            public int face_array_size
            {
                get
                {
                    return face_index_count / 3;
                }
            }

            [ArraySize("bone_array_size")]
            public Bone[] bones;

            [ArraySize("bone_array_size")]
            public int[] bone_id;

            [VersionSelectAttribute("vertex_chunk_length", 48, typeof(Vertex48))]
            [VersionSelectAttribute("vertex_chunk_length", 44, typeof(Vertex44))]
            [VersionSelectAttribute("vertex_chunk_length", 40, typeof(Vertex40))]
            [VersionSelectAttribute("vertex_chunk_length", 36, typeof(Vertex36))]
            [ArraySize("vertex_array_size")]
            public Vertex[] vertices;

            [ArraySize("face_array_size")]
            public Face[] faces;
        }

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static unsafe void Main(string[] args) {

            using (FormattedReader fr = new FormattedReader(@"C:\T3fun\AikaOnline\Mesh\PR03060202.msh"))
            {
                var msh = fr.Read<MSHFormat>();
            }

            VersionInfo[] vers = new VersionInfo[]{
                
                new VersionInfo(){
                    sourcePath=@"C:\T3fun\AikaOnline",
                    destPath=@"data",
                    cultureNum=0,
                    encoding="euc-KR",
                    indexsuffix="",
                    allowKorean = false,
                    COOL_PAGE_TITLE = "Aika DB of Blessed Heaven",
                    SIMPLE_PAGE_TITLE = "Aika DB of Grey Hawk",
                    trans = TranslationNA.table,
                    title = "AIKA Online (North America) Database",
                    reinforceVersion = 2
                },

                new VersionInfo(){
                    sourcePath=@"G:\HanbitOn\AIKA",
                    destPath=@"kr",
                    cultureNum=1,
                    encoding="euc-KR",
                    indexsuffix="-kr",
                    allowKorean = true,
                    COOL_PAGE_TITLE = "Aika DB",
                    SIMPLE_PAGE_TITLE = "Aika DB",
                    trans = new Dictionary<string,string>(),
                    title = "AIKA KR",
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    sourcePath=@"D:\HANBIT\AIKA",
                    destPath=@"jp",
                    cultureNum=2,
                    encoding="shift_jis",
                    indexsuffix="-jp",
                    allowKorean = true,
                    COOL_PAGE_TITLE = "永遠のAIKA DB",
                    SIMPLE_PAGE_TITLE = "天使のAIKA DB",
                    trans = TranslationJP.table,
                    title = "戦場のエルター(AIKA 日本版)",
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    sourcePath="",
                    destPath="",
                    cultureNum=3,
                    encoding="",
                    allowKorean = true,
                    COOL_PAGE_TITLE = "",
                    SIMPLE_PAGE_TITLE = "",
                    trans = new Dictionary<string,string>(),
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    sourcePath="",
                    destPath="",
                    cultureNum=4,
                    encoding="",
                    allowKorean = true,
                    COOL_PAGE_TITLE = "",
                    SIMPLE_PAGE_TITLE = "",
                    trans = new Dictionary<string,string>(),
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    sourcePath="",
                    destPath="",
                    cultureNum=5,
                    encoding="",
                    allowKorean = true,
                    COOL_PAGE_TITLE = "",
                    SIMPLE_PAGE_TITLE = "",
                    trans = new Dictionary<string,string>(),
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    sourcePath=@"D:\OnGame\AIKA",
                    destPath="br",
                    cultureNum=6,
                    encoding="latin1",
                    indexsuffix="-br",
                    allowKorean = false,
                    COOL_PAGE_TITLE = "Aika DB da Relíquia Selada",
                    SIMPLE_PAGE_TITLE = "Aika DB da Vida",
                    trans = TranslationBR.table,
                    title = "AIKA Database Brasil",
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    sourcePath="",
                    destPath="",
                    cultureNum=7,
                    encoding="",
                    allowKorean = true,
                    trans = new Dictionary<string,string>(),
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    // no longer extant
                    sourcePath= @"f:\t3fun\AIKAGlobal",
                    destPath=@"data",
                    cultureNum=8,
                    encoding="euc-KR",
                    indexsuffix="",
                    allowKorean = true,
                    trans = new Dictionary<string,string>(),
                    title = "AIKA GB",
                    reinforceVersion = 1
                },

                new VersionInfo(){
                    // no longer extant
                    sourcePath= @"I:\Asiasoft Online\AikaSEA",
                    destPath=@"data",
                    cultureNum=9,
                    encoding="euc-KR",
                    indexsuffix="",
                    allowKorean = true,
                    COOL_PAGE_TITLE = "Aika DB of Abyss",
                    SIMPLE_PAGE_TITLE = "Aika DB of Divine Oracle",
                    trans = new Dictionary<string,string>(),
                    title = "AIKA SEA Database",
                    reinforceVersion = 1
                },
                
                new VersionInfo(){
                    sourcePath= @"G:\Asiasoft\AikaIndo",
                    destPath=@"indo",
                    cultureNum=10,
                    encoding="euc-KR",
                    indexsuffix="-id",
                    allowKorean = false,
                    COOL_PAGE_TITLE = "Aika DB yang Maha Mulia",
                    SIMPLE_PAGE_TITLE = "Aika DB yang Bersatu",
                    trans = TranslationID.table,
                    title = "AIKA Indonesia Database",
                    reinforceVersion = 1
                }
            };


            int selection = int.Parse(args[0]);

            reinforceVersion = vers[selection].reinforceVersion;
            sourcePath = vers[selection].sourcePath;
            destPath = vers[selection].destPath;
            cultureNum = vers[selection].cultureNum;
            encoding = vers[selection].encoding;
            indexsuffix = vers[selection].indexsuffix;
            allowKorean = vers[selection].allowKorean;
            trans = vers[selection].trans;
            title = vers[selection].title;
            COOL_PAGE_TITLE = vers[selection].COOL_PAGE_TITLE;
            SIMPLE_PAGE_TITLE = vers[selection].SIMPLE_PAGE_TITLE;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);


            Directory.GetFiles(sourcePath + @"\Texture", "*.jit").ToList().ForEach(x =>
            {
                JitConverter.GetImage(x).Save(x + ".png");
            });


            var a = SiteGenerator.nfi;

            //Crypto.DecipherReinforce(sourcePath + @"\UI\PI.bin", Crypto.CT2);
            //Crypto.DecipherReinforce2(sourcePath + @"\UI\SaleType.bin", Crypto.CT6);

            Application.Run(new Skills());

            //SQLiteGenerator.SQLiteGenerator.Import();
            //return;


            //Directory.GetFiles(sourcePath + @"\Effect", "*.jit").ToList().ForEach(x =>
            //{
            //    JitConverter.GetImage(x).Save(x + ".png");
            //});
            //return;
            //Application.Run(new ItemList());


            Crypto.DecipherReinforce2(sourcePath + @"\SL.bin", Crypto.CT4);


            Directory.GetFiles(sourcePath + @"\UI", "*.jit").ToList().ForEach(x =>
            {
                JitConverter.GetImage(x).Save(x + ".png");
            });


            File.Copy(sourcePath + @"\UI\skillicons.jit.png", sourcePath + @"\UI\skillicons1.jit.png", true);
            File.Copy(sourcePath + @"\UI\skillicons02.jit.png", sourcePath + @"\UI\skillicons2.jit.png", true);

            Crypto.DecipherReinforce(sourcePath + @"\UI\Recipe.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\RecipeRate.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\Reinforce.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\Reinforce2.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\Reinforce3.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\ReinforceA01.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\ReinforceW01.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\Easing.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\SetItem.bin", Crypto.CT2);
            Crypto.DecipherReinforce(sourcePath + @"\UI\dialog.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\MakeItems.bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\UI\SetItem.bin", Crypto.CT2);
            Crypto.DecipherReinforce(sourcePath + @"\UI\MN.bin", Crypto.CT3);
            Crypto.DecipherReinforce(sourcePath + @"\UI\quest.bin", Crypto.CT1);

            Crypto.DecipherReinforce(sourcePath + @"\SkillData" + cultureNum + ".bin", Crypto.CT1);
            Crypto.DecipherReinforce(sourcePath + @"\ItemList" + cultureNum + ".bin", Crypto.CT2);



            SiteGenerator.Import();

            // write translation table
            using (StreamWriter sw = new StreamWriter(Path.Combine(webpath, @"translation" + indexsuffix + ".cs")))
            {
                sw.WriteLine("using System;");
                sw.WriteLine("class translation" + destPath + " {");

                sw.WriteLine("  public static Dictionary<string,string> table = new Dictionary<string,string>();");

                sw.WriteLine("  static translation" + destPath + " {");

                foreach (string str in Translation.strings)
                {
                    sw.WriteLine("      table[\"{0}\"] = \"{0}\";", str);
                }
                sw.WriteLine("  }");
                sw.WriteLine("}");
            }

            //SiteGenerator.MakePriceList();
		}




        //Crypto.DecipherReinforce(@"C:\t3fun\AIKAGlobal\UI\ReinforceA01.bin", Crypto.CT1);
        //Crypto.EncipherReinforce(@"I:\backups\aphrodite\Downloads\JitOpener.7z\JitOpener\ReinforceA01.bin.dec");
        //Crypto.DecipherReinforce(@"C:\Downloads\ReinforceW01.bin");

        //string path = @"W:\www\aikadb";

        //PropertyInfo[] pis = typeof(FileFormats.Item).GetProperties();

        //using (StreamWriter sw = new StreamWriter(Path.Combine(path,"index.html")))
        //{
        //    sw.WriteLine("<html>");
        //    sw.WriteLine("<body>");

        //    sw.WriteLine("<table>");


        //    sw.WriteLine("<tr>");
        //    foreach (var pi in pis)
        //    {
        //        sw.Write("<td>{0}</td>", pi.Name);
        //    }
        //    sw.WriteLine("</tr>");

        //    for (int i = 0; i < FileFormats.itemlistbin.items.Length; i++)
        //    {
        //        FileFormats.Item t = FileFormats.itemlistbin.items[i];

        //        sw.WriteLine("<tr>");

        //        foreach (var pi in pis)
        //        {
        //            sw.Write("<td>");
        //            if (pi.PropertyType == typeof(Image))
        //            {
        //                Image img = (Image)pi.GetValue(t, null);
        //                string relative = "images/" + i + ".png";
        //                string filename = Path.Combine(path, relative);
        //                img.Save(filename, ImageFormat.Png);
        //                sw.Write("<img src=\"{0}\" />", relative);
        //            }
        //            else
        //            {
        //                sw.Write(pi.GetValue(t, null).ToString());
        //            }
        //            sw.Write("</td>");
        //        }


        //        sw.WriteLine("</tr>");
        //    }

        //    sw.WriteLine("</table>");

        //    sw.WriteLine("</body>");
        //    sw.WriteLine("</html>");
        //}



        //Application.Run(new ObjPos());

        //SQLite sql = new SQLite();
        //sql.OpenDatabase("aikadb.sqlite3");
	}
}
