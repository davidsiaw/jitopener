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
    public partial class Skills : Form
    {
        public Skills()
        {
            InitializeComponent();

            DataGridViewImageColumn dgvic = new DataGridViewImageColumn();
            dgvic.Name = "Icon";
            dataGridView1.Columns.Add(dgvic);
            dataGridView1.Columns.Add("Name", "name");
            dataGridView1.Columns.Add("Type", "type");
            dataGridView1.Columns.Add("Profession", "Profession");
            dataGridView1.Columns.Add("Level", "Level");

            dataGridView1.Columns.Add("m2", "m2");
            dataGridView1.Columns.Add("m3", "m3");
            dataGridView1.Columns.Add("m4", "m4");
            dataGridView1.Columns.Add("m6", "m6");
            dataGridView1.Columns.Add("m7", "m7");

            for (int i = 0; i < 65; i++)
            {
                dataGridView1.Columns.Add(i.ToString(), i.ToString());
            }

            Thread t = new Thread(new ThreadStart(() =>
            {
                FileFormats.skilldatabin.skills.Where(x => x.iconid != 0).ToList().ForEach(x =>
                {
                    Invoke(new Action(() =>
                    {
                        int row = dataGridView1.Rows.Add();
                        dataGridView1.Rows[row].Height = 44;
                        dataGridView1.Rows[row].Cells[0].Value = x.Image;
                        dataGridView1.Rows[row].Cells[1].Value = x.Name;
                        dataGridView1.Rows[row].Cells[2].Value = x.Type;
                        dataGridView1.Rows[row].Cells[3].Value = x.skillprofession;
                        dataGridView1.Rows[row].Cells[4].Value = x.skillLevel;

                        dataGridView1.Rows[row].Cells[5].Value = x.skillLevelCap;
                        dataGridView1.Rows[row].Cells[6].Value = x.mystery3;
                        dataGridView1.Rows[row].Cells[7].Value = x.skillPointCost;
                        dataGridView1.Rows[row].Cells[8].Value = x.guildSkill;
                        dataGridView1.Rows[row].Cells[9].Value = x.mystery7;

                        for (int i = 0; i < 65; i++)
                        {
                            dataGridView1.Rows[row].Cells[8 + i].Value = x.mysteries[i];
                        }
                    }));
                });
            }));
            t.Start();
        }
    }
}
