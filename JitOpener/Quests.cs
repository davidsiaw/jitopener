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
    public partial class Quests : Form
    {
        public Quests()
        {
            InitializeComponent();
        }

        private void Quests_Load(object sender, EventArgs e)
        {

            PropertyInfo[] pis = typeof(FileFormats.Quest).GetProperties();


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




                FileFormats.QuestBin qb = FileFormats.questbin;
                for (int i = 0; i < qb.quests.Length; i++)
                {

                    FileFormats.Quest item = qb.quests[i];

                    if (item.Name.Length == 0)
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
