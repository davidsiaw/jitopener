using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace JitOpener
{
    public partial class ObjPos : Form
    {
        public ObjPos()
        {
            InitializeComponent();

            PropertyInfo[] pis = typeof(FileFormats.ObjPos).GetProperties();


            foreach (var pi in pis)
            {
                if (pi.PropertyType == typeof(Image))
                {
                    dataGridView1.Columns.Add(new DataGridViewImageColumn());
                }
                else
                {
                    dataGridView1.Columns.Add(pi.Name, pi.Name);
                }
            }


            Thread t = new Thread(new ThreadStart(() =>
            {




                FileFormats.ObjPosBin qb = FileFormats.objposbin;
                for (int i = 0; i < qb.objects.Length; i++)
                {

                    FileFormats.ObjPos item = qb.objects[i];

                    if (item.FakeName.Length == 0)
                    {
                        continue;
                    }

                    List<object> str = new List<object>();

                    foreach (var pi in pis)
                    {
                        if (pi.PropertyType == typeof(Image))
                        {
                            str.Add(pi.GetValue(item, null));
                        }
                        else
                        {
                            str.Add(pi.GetValue(item, null).ToString());
                        }
                    }


                    Invoke(new Action(() =>
                    {
                        int rownum = dataGridView1.Rows.Add(str.ToArray());
                        dataGridView1.Rows[rownum].Height = 44;
                    }));
                }



            }));

            t.Start();
        }
    }
}
