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
    public partial class MobPos : Form
    {
        public MobPos()
        {
            InitializeComponent();
        }

        private void MobPos_Load(object sender, EventArgs e)
        {

            PropertyInfo[] pis = typeof(FileFormats.MobPos).GetProperties();


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




                FileFormats.MobPosBin qb = FileFormats.mobposbin;
                for (int i = 0; i < qb.entries.Length; i++)
                {

                    FileFormats.MobPos item = qb.entries[i];

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
