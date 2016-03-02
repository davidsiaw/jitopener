using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BlueBlocksLib.FileAccess;
using System.IO;
using System.Threading;
using System.Reflection;

namespace JitOpener {
	public partial class ItemList : Form {



		public ItemList() {
			InitializeComponent();

			PropertyInfo[] pis = typeof(FileFormats.Item).GetProperties();


			foreach (var pi in pis) {
				if (pi.PropertyType == typeof(Image)) {
					dataGridView1.Columns.Add(new DataGridViewImageColumn());
				} else {
					dataGridView1.Columns.Add(pi.Name, pi.Name);
				}
			}
			

			for (int i = 0; i < 104; i++) {
                dataGridView1.Columns.Add("C" + i, i.ToString());
            }

            Thread t = new Thread(new ThreadStart(() =>
            {




                FileFormats.ItemListBin il = FileFormats.itemlistbin;
                for (int i = 0; i < il.items.Length; i++)
                {

                    FileFormats.Item item = il.items[i];

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

                    for (int j = 0; j < item.things.Length; j++)
                    {
                        str.Add(item.things[j].ToString());
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

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

		}


	}
}
