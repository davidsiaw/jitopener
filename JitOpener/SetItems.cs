using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace JitOpener
{
    public partial class SetItems : Form
    {
        public SetItems()
        {
            InitializeComponent();

            dataGridView1.Columns.Add("name", "name");
            dataGridView1.Columns.Add("items", "items");
            dataGridView1.Columns.Add("effects", "effects");

            Thread t = new Thread(() =>
            {
                foreach (var set in FileFormats.setitembin.sets)
                {
                    if (set.Name.Length != 0)
                    {
                        Invoke(new Action(() => {
                            dataGridView1.Rows.Add(set.Name, string.Join("\r\n", set.SetItems.Select(x => x.Name)), string.Join("\r\n", set.SetEffects(SiteGenerator.GetSkillPageLink)));
                        }));
                    }
                }
            });
            t.Start();
        }

        private void SetItems_Load(object sender, EventArgs e)
        {
        }
    }
}
