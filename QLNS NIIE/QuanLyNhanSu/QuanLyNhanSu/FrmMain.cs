using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhanSu
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Are you sure you want to Log out?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                this.Close();
                Form1 f = new Form1();
                f.Show();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LayDSNS();
        }

        private void LayDSNS()
        {
            SqlConnection con = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
            try
            {
                con.Open();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = "SP_LayDSNS";
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Connection = con;
                da.Fill(dt);
                dtgDSNS.DataSource = dt;
                con.Close();
                dtgDSNS.Columns[0].Width = 35;
                dtgDSNS.Columns[0].HeaderText = "ID";
                dtgDSNS.Columns[1].Width = 130;
                dtgDSNS.Columns[1].HeaderText = "Name";
                dtgDSNS.Columns[2].Width = 80;
                dtgDSNS.Columns[2].HeaderText = "Sex";
                dtgDSNS.Columns[3].Width = 80;
                dtgDSNS.Columns[3].HeaderText = "Date of Birth";
                dtgDSNS.Columns[4].Width = 135;
                dtgDSNS.Columns[4].HeaderText = "Email";
                dtgDSNS.Columns[5].Width = 80;
                dtgDSNS.Columns[5].HeaderText = "Address";
                dtgDSNS.Columns[6].Width = 70;
                dtgDSNS.Columns[6].HeaderText = "Phone";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboSapXep_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSapXep.SelectedIndex == 0)
            {
                this.dtgDSNS.Sort(this.dtgDSNS.Columns["HoTen"], ListSortDirection.Ascending);
            }
            else
            {
                this.dtgDSNS.Sort(this.dtgDSNS.Columns["ID"], ListSortDirection.Ascending);
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string rowFilter = string.Format("{0} like '{1}'", "HoTen", "*" + txtTimKiem.Text + "*");
            (dtgDSNS.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "")
            {
                LayDSNS();
            }
        }

        private void Reset()
        {
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtHoTen.Text = "";
            txtID.Text = "";
            txtPhone.Text = "";
            rdoNam.Checked = false;
            rdoNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
        }

        private bool KiemTraThongTin()
        {
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Please enter the employee's first and last name.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHoTen.Focus();
                return false;
            }
            if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Please enter employee's address.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return false;
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Please enter employee's email.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return false;
            }
            if (rdoNam.Checked == false && rdoNu.Checked == false)
            {
                MessageBox.Show("Please select employee gender.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (txtPhone.Text == "")
            {
                MessageBox.Show("Please enter employee phone number.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTraThongTin())
            {
                try
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SP_ThemNhanVien";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = txtHoTen.Text;
                    if(rdoNam.Checked == true)
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNam.Text;
                    }
                    else
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNu.Text;
                    }
                    cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value = dtpNgaySinh.Text;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text;
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = txtPhone.Text;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LayDSNS();
                    MessageBox.Show("New employee successfully added.", "Announcement", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtID_Enter(object sender, EventArgs e)
        {
            if (txtID.Text == "Add new without ID")
            {
                txtID.Clear();
                txtID.ForeColor = SystemColors.Highlight;
            }
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                txtID.Text = "Add new without ID";
                txtID.ForeColor = SystemColors.InactiveCaption;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if(txtID.Text == "" || txtID.Text == "Add new without ID")
            {
                MessageBox.Show("Please enter the employee ID to be corrected.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                txtID.SelectAll();
            }
            else if (KiemTraThongTin())
            {
                try
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SP_SuaNhanVien";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(txtID.Text);
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = txtHoTen.Text;
                    if (rdoNam.Checked == true)
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNam.Text;
                    }
                    else
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNu.Text;
                    }
                    cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value = dtpNgaySinh.Text;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text;
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = txtPhone.Text;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LayDSNS();
                    MessageBox.Show("Successfully edited employee.", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dtgDSNS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dtgDSNS.Rows[e.RowIndex];
            txtID.Text = Convert.ToString(row.Cells["ID"].Value);
            txtHoTen.Text = Convert.ToString(row.Cells["HoTen"].Value);
            dtpNgaySinh.Text = Convert.ToString(row.Cells["NgaySinh"].Value);
            txtDiaChi.Text = Convert.ToString(row.Cells["DiaChi"].Value);
            txtEmail.Text = Convert.ToString(row.Cells["Email"].Value);
            string GioiTinh = Convert.ToString(row.Cells["GioiTinh"].Value);
            if (GioiTinh.Trim() == "Nu")
            {
                rdoNu.Checked = true;
            }
            else
            {
                rdoNam.Checked = true;
            }
            txtPhone.Text = Convert.ToString(row.Cells["Phone"].Value);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "Add new without ID" || txtID.Text == "")
            {
                MessageBox.Show("Please enter the employee ID to be deleted.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
            }
            else
            {
                try
                {
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SP_XoaNhanVien";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(txtID.Text);
                   
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LayDSNS();
                    MessageBox.Show("Delete employee successfully.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
