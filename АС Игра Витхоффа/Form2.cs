using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АС_Игра_Витхоффа
{
    public partial class Form2 : Form
    {
        public Form2(List<string> hm)
        {
            InitializeComponent();
            listBox1.Items.Clear();
            foreach (string move in hm)
            {
                if (move != null)
                {
                    listBox1.Items.Add(move);
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}
