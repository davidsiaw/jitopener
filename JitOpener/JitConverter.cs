using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;
using Tao.DevIl;
using System.Drawing.Imaging;

namespace JitOpener
{
    class JitConverter
    {
        static bool devILinit = false;
        public static Bitmap DDSDataToBMP(byte[] DDSData)
        {
            if (!devILinit)
            {
                Il.ilInit();
                devILinit = true;
            }

            // Create a DevIL image "name" (which is actually a number)
            int img_name;
            Il.ilGenImages(1, out img_name);
            Il.ilBindImage(img_name);

            // Load the DDS file into the bound DevIL image
            bool a = Il.ilLoadL(Il.IL_DDS, DDSData, DDSData.Length);

            // Set a few size variables that will simplify later code

            int ImgWidth = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
            int ImgHeight = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
            Rectangle rect = new Rectangle(0, 0, ImgWidth, ImgHeight);

            // Convert the DevIL image to a pixel byte array to copy into Bitmap
            Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE);

            // Create a Bitmap to copy the image into, and prepare it to get data
            Bitmap bmp = new Bitmap(ImgWidth, ImgHeight);
            BitmapData bmd =
            bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            // Copy the pixel byte array from the DevIL image to the Bitmap
            Il.ilCopyPixels(0, 0, 0,
            Il.ilGetInteger(Il.IL_IMAGE_WIDTH),
            Il.ilGetInteger(Il.IL_IMAGE_HEIGHT),
            1, Il.IL_BGRA, Il.IL_UNSIGNED_BYTE,
            bmd.Scan0);

            // Clean up and return Bitmap
            Il.ilDeleteImages(1, ref img_name);
            bmp.UnlockBits(bmd);
            return bmp;
        }

        public static void MakeBlinker(string save, Image toBlink)
        {
            if (!Directory.Exists(Path.GetDirectoryName(save)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(save));
            }

            Bitmap b = new Bitmap(toBlink.Width, toBlink.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.Clear(Color.Transparent);
            }
            JitConverter.MakeGif(save, new Image[] { toBlink, b });
        }

        public static void MakeGif(string path, Image[] images)
        {
            //Variable declaration

            Image image;
            Byte[] buf1;
            Byte[] buf2;
            Byte[] buf3;
            //Variable declaration

          

            buf2 = new Byte[19];
            buf3 = new Byte[8];
            buf2[0] = 33;  //extension introducer
            buf2[1] = 255; //application extension
            buf2[2] = 11;  //size of block
            buf2[3] = 78;  //N
            buf2[4] = 69;  //E
            buf2[5] = 84;  //T
            buf2[6] = 83;  //S
            buf2[7] = 67;  //C
            buf2[8] = 65;  //A
            buf2[9] = 80;  //P
            buf2[10] = 69; //E
            buf2[11] = 50; //2
            buf2[12] = 46; //.
            buf2[13] = 48; //0
            buf2[14] = 3;  //Size of block
            buf2[15] = 1;  //
            buf2[16] = 0;  //
            buf2[17] = 0;  //
            buf2[18] = 0;  //Block terminator
            buf3[0] = 33;  //Extension introducer
            buf3[1] = 249; //Graphic control extension
            buf3[2] = 4;   //Size of block
            buf3[3] = 9;   //Flags: reserved, disposal method, user input, transparent color
            buf3[4] = 40;  //Delay time low byte
            buf3[5] = 0;   //Delay time high byte
            buf3[6] = 0; //Transparent color index
            buf3[7] = 0;   //Block terminator

            using (MemoryStream memoryStream = new MemoryStream())
            using (BinaryWriter binaryWriter = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                for (int picCount = 0; picCount < images.Length; picCount++)
                {
                    image = images[picCount];
                    image.Save(memoryStream, ImageFormat.Gif);
                    buf1 = memoryStream.ToArray();

                    if (picCount == 0)
                    {
                        //only write these the first time....
                        binaryWriter.Write(buf1, 0, 781); //Header & global color table
                        binaryWriter.Write(buf2, 0, 19); //Application extension
                    }

                    binaryWriter.Write(buf3, 0, 8); //Graphic extension
                    binaryWriter.Write(buf1, 789, buf1.Length - 790); //Image data

                    if (picCount == images.Length - 1)
                    {
                        //only write this one the last time....
                        binaryWriter.Write(";"); //Image terminator
                    }

                    memoryStream.SetLength(0);
                }
            }

        }

        public static string MakeMapWithMarkers(string imagepath, string outputpath, string title, PointF[] points)
        {

            using (Image b = Bitmap.FromFile(imagepath))
            {
                string actualpath = outputpath;

                int zoomlevel = 0;
                int zoom = 0;

                if (!Directory.Exists(actualpath))
                {
                    Directory.CreateDirectory(actualpath);
                    while ((b.Width >> zoomlevel) / 64 > 0)
                    {

                        for (int x = 0; x < zoom && x * (b.Width >> zoomlevel) < b.Width; x++)
                        {
                            for (int y = 0; y < zoom && y * (b.Height >> zoomlevel) < b.Height; y++)
                            {
                                using (Bitmap bmp = new Bitmap(128, 128))
                                {
                                    using (Graphics g = Graphics.FromImage(bmp))
                                    {
                                        g.DrawImage(b, new Rectangle(0, 0, 128, 128), new Rectangle(x * (b.Width >> zoomlevel), y * (b.Height >> zoomlevel), (b.Width >> zoomlevel), (b.Height >> zoomlevel)), GraphicsUnit.Pixel);
                                    }

                                    string tile = Path.Combine(actualpath, "z" + zoomlevel, "x" + x, "y" + y + ".png");
                                    Console.WriteLine("Zoom {0} X {1} Y {2}", zoomlevel, x, y);
                                    if (!Directory.Exists(Path.GetDirectoryName(tile)))
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(tile));
                                    }
                                    bmp.Save(tile, ImageFormat.Png);
                                }
                            }
                        }

                        zoomlevel++;
                        zoom = 1 << zoomlevel;
                    }
                }
                else
                {
                    while ((b.Width >> zoomlevel) / 64 > 0)
                    {
                        zoomlevel++;
                        zoom = 1 << zoomlevel;
                    }
                }
                return (@"
<div id=""map_canvas"" style=""width:500px; height:500px""></div>

<script type=""text/javascript"">
var moonTypeOptions = {
getTileUrl: function(coord, zoom) {
var normalizedCoord = getNormalizedCoord(coord, zoom);
if (!normalizedCoord) {
return null;
}
var bound = Math.pow(2, zoom);
return ""z"" + zoom + ""/x"" + normalizedCoord.x + ""/y"" +
(normalizedCoord.y ) + "".png"";
},
tileSize: new google.maps.Size(128, 128),
maxZoom: " + (zoomlevel - 1) + @",
minZoom: 2,
name: """ + title + @"""
};


var moonMapType = new google.maps.ImageMapType(moonTypeOptions);

function initialize() {
var myLatlng = new google.maps.LatLng(32,32);
var mapOptions = {
backgroundColor: ""#000"",
center: myLatlng,
zoom: 3,
streetViewControl: false,
mapTypeControlOptions: {
mapTypeIds: [""reshanta""]
}
};

moonMapType.projection = new NullProjection();

var map = new google.maps.Map(document.getElementById(""map_canvas""),
mapOptions);
map.mapTypes.set('reshanta', moonMapType);
map.setMapTypeId('reshanta');

var locations = [];
" + string.Join("\n", points.Select(x => "locations.push({lat:" + x.Y + ", lon:" + x.X + "});")) + @"

for (var i = 0; i < locations.length; i++) {
var p = locations[i];
var myLatLng = new google.maps.LatLng(p.lat, p.lon);
var marker = new google.maps.Marker({
position: myLatLng,
map: map,
title: ""["" + p.lon + "","" + p.lat + ""]""
});

}

}

// Normalizes the coords that tiles repeat across the x axis (horizontally)
// like the standard Google map tiles.
function getNormalizedCoord(coord, zoom) {
var y = coord.y;
var x = coord.x;

// tile range in one direction range is dependent on zoom level
// 0 = 1 tile, 1 = 2 tiles, 2 = 4 tiles, 3 = 8 tiles, etc
var tileRange = 1 << zoom;

// don't repeat across y-axis (vertically)
if (y < 0 || y >= tileRange) {
return null;
}

// don't repeat across x-axis (vertically)
if (x < 0 || x >= tileRange) {
return null;
}


return {
x: x,
y: y
};
}

initialize();
</script>
");

            }
        }

        public static unsafe Image GetImage(string jitfile)
        {

            Console.WriteLine("Converting image: {0}", jitfile);
            using (FileStream fs = new FileStream(jitfile, FileMode.Open))
            {

                JITHEADER jitheader;


                byte[] jithdr = new byte[0xc];
                fs.Read(jithdr, 0, jithdr.Length);

                fixed (byte* ptr = &jithdr[0])
                {
                    jitheader = (JITHEADER)Marshal.PtrToStructure((IntPtr)ptr, typeof(JITHEADER));
                }

                if (jitheader.magicNumber == JITSIGN.JT20)
                {
                    byte[] tgaBytes = new byte[fs.Length - 4];
                    fs.Seek(4, SeekOrigin.Begin);
                    fs.Read(tgaBytes, 0, tgaBytes.Length);

                    if (!devILinit)
                    {
                        Il.ilInit();
                        devILinit = true;
                    }

                    // Create a DevIL image "name" (which is actually a number)
                    int img_name;
                    Il.ilGenImages(1, out img_name);
                    Il.ilBindImage(img_name);

                    // Load the DDS file into the bound DevIL image
                    bool a = Il.ilLoadL(Il.IL_TGA, tgaBytes, tgaBytes.Length);

                    // Set a few size variables that will simplify later code

                    int ImgWidth = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                    int ImgHeight = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                    Rectangle rect = new Rectangle(0, 0, ImgWidth, ImgHeight);

                    // Convert the DevIL image to a pixel byte array to copy into Bitmap
                    Il.ilConvertImage(Il.IL_BGRA, Il.IL_UNSIGNED_BYTE);

                    // Create a Bitmap to copy the image into, and prepare it to get data
                    Bitmap bmp = new Bitmap(ImgWidth, ImgHeight);
                    BitmapData bmd =
                    bmp.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                    // Copy the pixel byte array from the DevIL image to the Bitmap
                    Il.ilCopyPixels(0, 0, 0,
                    Il.ilGetInteger(Il.IL_IMAGE_WIDTH),
                    Il.ilGetInteger(Il.IL_IMAGE_HEIGHT),
                    1, Il.IL_BGRA, Il.IL_UNSIGNED_BYTE,
                    bmd.Scan0);

                    // Clean up and return Bitmap
                    Il.ilDeleteImages(1, ref img_name);
                    bmp.UnlockBits(bmd);
                    return bmp;
                }


                DDS_HEADER header = new DDS_HEADER();
                header.magicNumber = new byte[] { 68, 68, 83, 32 };
                header.ddspf.dwSize = 32;
                header.dwSize = 124;
                header.dwHeight = jitheader.dwHeight;
                header.dwWidth = jitheader.dwWidth;
                header.ddspf.dwFlags = DWFLAGS.DDPF_FOURCC;


                switch (jitheader.magicNumber)
                {
                    case JITSIGN.JT31:
                        header.ddspf.dwFourCC = DWFOURCC.DXT1;
                        break;
                    case JITSIGN.JT32:
                        header.ddspf.dwFourCC = DWFOURCC.DXT2;
                        break;
                    case JITSIGN.JT33:
                        header.ddspf.dwFourCC = DWFOURCC.DXT3;
                        break;
                    case JITSIGN.JT34:
                        header.ddspf.dwFourCC = DWFOURCC.DXT4;
                        break;
                    case JITSIGN.JT35:
                        header.ddspf.dwFourCC = DWFOURCC.DXT5;
                        break;
                    case JITSIGN.JT41:
                        header.ddspf.dwFourCC = DWFOURCC.DXT1;
                        break;
                }


                header.dwMipMapCount = 0;
                //header.dwPitchOrLinearSize = 1048576;
                header.dwHeaderFlags =
                DWHEADERFLAGS.DDSD_CAPS |
                DWHEADERFLAGS.DDSD_HEIGHT |
                DWHEADERFLAGS.DDSD_WIDTH;
                header.dwSurfaceFlags = DWSURFACEFLAGS.DDSCAPS_TEXTURE;
                header.dwReserved1 = new uint[11];
                header.dwReserved2 = new uint[3];


                byte[] stuff = new byte[fs.Length - 0xc];
                fs.Read(stuff, 0, stuff.Length);

                using (MemoryStream ms = new MemoryStream())
                {

                    byte[] hdr = new byte[0x80];
                    fixed (byte* ptr = &hdr[0])
                    {
                        Marshal.StructureToPtr(header, (IntPtr)ptr, false);
                    }
                    ms.Write(hdr, 0, hdr.Length);
                    ms.Write(stuff, 0, stuff.Length);
                    return DDSDataToBMP(ms.GetBuffer());
                }




            }
        }

        unsafe public static void ConvertJITToDDS(string jitfile)
        {



            using (FileStream fs = new FileStream(jitfile, FileMode.Open))
            {

                JITHEADER jitheader;


                byte[] jithdr = new byte[0xc];
                fs.Read(jithdr, 0, jithdr.Length);

                fixed (byte* ptr = &jithdr[0])
                {
                    jitheader = (JITHEADER)Marshal.PtrToStructure((IntPtr)ptr, typeof(JITHEADER));
                }

                byte[] stuff = new byte[fs.Length - 0xc];
                fs.Read(stuff, 0, stuff.Length);


                using (FileStream dest = new FileStream(jitfile + ".dds", FileMode.Create))
                {

                    DDS_HEADER header = new DDS_HEADER();
                    header.magicNumber = new byte[] { 68, 68, 83, 32 };
                    header.ddspf.dwSize = 32;
                    header.dwSize = 124;
                    header.dwHeight = jitheader.dwHeight;
                    header.dwWidth = jitheader.dwHeight;
                    header.ddspf.dwFlags = DWFLAGS.DDPF_FOURCC;

                    switch (jitheader.magicNumber)
                    {
                        case JITSIGN.JT31:
                            header.ddspf.dwFourCC = DWFOURCC.DXT1;
                            break;
                        case JITSIGN.JT32:
                            header.ddspf.dwFourCC = DWFOURCC.DXT2;
                            break;
                        case JITSIGN.JT33:
                            header.ddspf.dwFourCC = DWFOURCC.DXT3;
                            break;
                        case JITSIGN.JT34:
                            header.ddspf.dwFourCC = DWFOURCC.DXT4;
                            break;
                        case JITSIGN.JT35:
                            header.ddspf.dwFourCC = DWFOURCC.DXT5;
                            break;
                        case JITSIGN.JT41:
                            header.ddspf.dwFourCC = DWFOURCC.DXT1;
                            break;
                    }

                    header.dwMipMapCount = 0;
                    //header.dwPitchOrLinearSize = 1048576;
                    header.dwHeaderFlags =
                    DWHEADERFLAGS.DDSD_CAPS |
                    DWHEADERFLAGS.DDSD_HEIGHT |
                    DWHEADERFLAGS.DDSD_WIDTH;
                    header.dwSurfaceFlags = DWSURFACEFLAGS.DDSCAPS_TEXTURE;
                    header.dwReserved1 = new uint[11];
                    header.dwReserved2 = new uint[3];

                    byte[] hdr = new byte[0x80];
                    fixed (byte* ptr = &hdr[0])
                    {
                        Marshal.StructureToPtr(header, (IntPtr)ptr, false);
                    }

                    dest.Write(hdr, 0, hdr.Length);
                    dest.Write(stuff, 0, stuff.Length);


                }

            }
        }

        struct JITHEADER
        {
            public JITSIGN magicNumber;
            public uint dwWidth;
            public uint dwHeight;

        }

        enum JITSIGN
        {
            JT31 = 0x3133544A,
            JT32 = 0x3233544A,
            JT33 = 0x3333544A,
            JT34 = 0x3433544A,
            JT35 = 0x3533544A,
            JT41 = 0x3134544A,
            JT20 = 0x3032544A, // TGA
        }

        enum DWFLAGS
        {
            DDPF_FOURCC = 4
        }


        // DDSD_CAPS Required in every .dds file. 0x1
        //DDSD_HEIGHT Required in every .dds file. 0x2
        //DDSD_WIDTH Required in every .dds file. 0x4
        //DDSD_PITCH Required when pitch is provided for an uncompressed texture. 0x8
        //DDSD_PIXELFORMAT Required in every .dds file. 0x1000
        //DDSD_MIPMAPCOUNT Required in a mipmapped texture. 0x20000
        //DDSD_LINEARSIZE Required when pitch is provided for a compressed texture. 0x80000
        //DDSD_DEPTH Required in a depth texture. 0x800000

        // DDSCAPS_COMPLEX Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture). 0x8
        //DDSCAPS_MIPMAP Optional; should be used for a mipmap. 0x400000
        //DDSCAPS_TEXTURE Required 0x1000

        [Flags]
        enum DWSURFACEFLAGS
        {
            DDSCAPS_COMPLEX = 0x8,
            DDSCAPS_MIPMAP = 0x400000,
            DDSCAPS_TEXTURE = 0x1000,
        }

        [Flags]
        enum DWHEADERFLAGS
        {
            DDSD_CAPS = 0x1,
            DDSD_HEIGHT = 0x2,
            DDSD_WIDTH = 0x4,
            DDSD_PITCH = 0x8,
            DDSD_PIXELFORMAT = 0x1000,
            DDSD_MIPMAPCOUNT = 0x20000,
            DDSD_LINEARSIZE = 0x80000,
            DDSD_DEPTH = 0x800000,
        }

        enum DWFOURCC
        {
            DXT1 = 0x31545844,
            DXT2 = 0x32545844,
            DXT3 = 0x33545844,
            DXT4 = 0x34545844,
            DXT5 = 0x35545844,
            DXTB = 0x3B545844
        }


        [StructLayout(LayoutKind.Sequential)]
        struct DDS_PIXELFORMAT
        {
            public uint dwSize;
            public DWFLAGS dwFlags;
            public DWFOURCC dwFourCC;
            public uint dwRGBBitCount;
            public uint dwRBitMask;
            public uint dwGBitMask;
            public uint dwBBitMask;
            public uint dwABitMask;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DDS_HEADER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] magicNumber;
            public uint dwSize;
            public DWHEADERFLAGS dwHeaderFlags;
            public uint dwHeight;
            public uint dwWidth;
            public uint dwPitchOrLinearSize;
            public uint dwDepth;
            public uint dwMipMapCount;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11, ArraySubType = UnmanagedType.U4)]
            public uint[] dwReserved1;
            public DDS_PIXELFORMAT ddspf;
            public DWSURFACEFLAGS dwSurfaceFlags;
            public uint dwCubemapFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] dwReserved2;
        }


    }
}
