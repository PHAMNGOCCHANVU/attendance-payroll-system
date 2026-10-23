using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuoiky
{
    public partial class FormTruongPhongTaiVu : Form
    {
        public int maNV; 
        public FormTruongPhongTaiVu(int maNV)
        {
            InitializeComponent();
            this.maNV = maNV;
        }

        public FormTruongPhongTaiVu()
        {
            InitializeComponent();
        }
    }
}
