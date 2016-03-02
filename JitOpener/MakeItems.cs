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
    public partial class MakeItems : Form
    {
        public MakeItems()
        {
            InitializeComponent();

            var imgcol = new DataGridViewImageColumn();
            imgcol.HeaderText = "Craft";
            dataGridView1.Columns.Add(imgcol);
            dataGridView1.Columns.Add("Product Name", "Product Name");

            dataGridView1.Columns.Add("Cost", "Cost");
            dataGridView1.Columns.Add("Superior Chance %", "Superior Chance %");

            for (int i = 0; i < 12; i++)
            {
                imgcol = new DataGridViewImageColumn();
                imgcol.HeaderText = "Ingredient " + i;
                dataGridView1.Columns.Add(imgcol);
                dataGridView1.Columns.Add("Ingredient " + i + " Name", "Ingredient " + i + " Name");
            }


            Thread t = new Thread(new ThreadStart(() =>
            {

                foreach (var recipe in FileFormats.makeitemsbin.recipes)
                {
                    if (recipe.createditemid == 0)
                    {
                        continue;
                    }

                    List<object> str = new List<object>();

                    str.Add(recipe.CreatedItem.Image);
                    str.Add(recipe.CreatedItem.Name);
                    str.Add(recipe.cost);
                    str.Add(((double)recipe.superiorChance / (double)10));

                    for (int i = 0; i < 12; i++)
                    {
                        str.Add(recipe.Ingredients[i].Key.Image);
                        str.Add(recipe.Ingredients[i].Value + "x " + recipe.Ingredients[i].Key.Name);
                    }

                    Invoke(new Action(() =>
                    {
                        int r = dataGridView1.Rows.Add(str.ToArray());
                        dataGridView1.Rows[r].Height = 44;
                    }));

                }
            }));
            t.Start();
        }

        private void MakeItems_Load(object sender, EventArgs e)
        {

        }
    }
}
