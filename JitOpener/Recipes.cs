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
    public partial class Recipes : Form
    {
        public Recipes()
        {
            InitializeComponent();

            var imgcol = new DataGridViewImageColumn();
            imgcol.HeaderText = "Recipe";
            dataGridView1.Columns.Add(imgcol);
            dataGridView1.Columns.Add("Recipe Name", "Recipe Name");

            imgcol = new DataGridViewImageColumn();
            imgcol.HeaderText = "Product";
            dataGridView1.Columns.Add(imgcol);
            dataGridView1.Columns.Add("Product Name", "Product Name");

            for (int i = 0; i < 12; i++)
            {
                imgcol = new DataGridViewImageColumn();
                imgcol.HeaderText = "Ingredient " + i;
                dataGridView1.Columns.Add(imgcol);
                dataGridView1.Columns.Add("Ingredient " + i + " Name", "Ingredient " + i + " Name");
            }


            Thread t = new Thread(new ThreadStart(() =>
            {

                foreach (var recipe in FileFormats.recipebin.recs)
                {
                    List<object> str = new List<object>();

                    str.Add(recipe.RecipeItem.Image);
                    str.Add(recipe.RecipeItem.Name);

                    str.Add(recipe.CreatedItem.Image);
                    str.Add(recipe.CreatedItem.Name);

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

        private void Recipes_Load(object sender, EventArgs e)
        {

        }
    }
}
