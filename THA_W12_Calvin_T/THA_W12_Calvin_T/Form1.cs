using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace THA_W12_Calvin_T
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        MySqlDataReader reader;
        string host = "server=localhost;uid=root;pwd=;database=premier_league;";
        string query = "";
        DataTable dtteam = new DataTable();
        DataTable dtnationality = new DataTable();
        DataTable dtplayer = new DataTable();
        DataTable dtmanager = new DataTable();
        DataTable dtteam2 = new DataTable();
        DataTable dtworking = new DataTable();
        DataTable dtteam3 = new DataTable();
        DataTable dtremoved = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(host);
            query = "select team_id as 'ID', team_name as 'Team Name' from team;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtteam);
            cmbbox_teamname.DataSource = dtteam;
            cmbbox_teamname.ValueMember = "ID";
            cmbbox_teamname.DisplayMember = "Team Name";
            query = "select nationality_id as 'ID', nation as 'Nation' from nationality;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtnationality);
            cmbbox_nationality.DataSource = dtnationality;
            cmbbox_nationality.ValueMember = "ID";
            cmbbox_nationality.DisplayMember = "Nation";
            query = "select team_id as 'ID', team_name as 'Team Name' from team;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtteam2);
            cmbbox_manager.DataSource = dtteam2;
            cmbbox_manager.ValueMember = "ID";
            cmbbox_manager.DisplayMember = "Team Name";
            query = "select team_id as 'ID', team_name as 'Team Name' from team;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtteam3);
            cmbbox_selectedteam.DataSource = dtteam3;
            cmbbox_selectedteam.ValueMember = "ID";
            cmbbox_selectedteam.DisplayMember = "Team Name";

        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            string player_id = tbox_id.Text;
            string player_name = tbox_name.Text;
            string team_number = tbox_teamnumber.Text;
            string nationality_id = cmbbox_nationality.SelectedValue.ToString();
            string playing_pos = cmbbox_pos.Text;
            string height = tbox_height.Text;
            string weight = tbox_weight.Text;
            string birthdate = dtp_birthdate.Value.ToString("yyyy-MM-dd");
            string team_id = cmbbox_teamname.SelectedValue.ToString();
            query = $"insert into player values ('{player_id}','{team_number}','{player_name}','{nationality_id}','{playing_pos}','{height}','{weight}','{birthdate}','{team_id}',1,0);";
            try
            {
                conn.Open();
                cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                updatedgvnew();
            }

        }

        private void tbox_teamnumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void tbox_height_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void tbox_weight_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }
        public void updatedgv()
        {
            dtremoved.Clear();
            query = $"select p.player_name as 'Name',n.nation as 'Nationality', p.playing_pos as 'Position', p.team_number as 'Number', p.height, p.weight, date_format(p.birthdate,'%Y-%m-%d') as 'Birthdate' from player p, nationality n where n.nationality_id = p.nationality_id and p.team_id = '{cmbbox_selectedteam.SelectedValue.ToString()}' and status = 1;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtremoved);
            dataGridView3.DataSource = dtremoved;
        }

        private void cmbbox_manager_SelectionChangeCommitted(object sender, EventArgs e)
        {
            updatedgv2();
        }
        public void updatedgvnew()
        {
            dtplayer.Clear();
            query = "select * from player;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtplayer);
            dataGridView1.DataSource = dtplayer;

        }
        private void btn_update_Click(object sender, EventArgs e)
        {
            query = $"update manager set working = 0 where manager_name = '{dataGridView1.SelectedCells[0].Value.ToString()}'";
            try
            {
                conn.Open();
                cmd = new MySqlCommand(query, conn);
                reader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
                updatedgv2();
            }
        }
        public void updatedgv2()
        {
            dtmanager.Clear();
            dtworking.Clear();
            query = $"select m.manager_name as 'Name', t.team_name as 'Team Name',date_format(m.birthdate,'%Y-%m-%d') as 'Birthdate',n.nation from manager m, team t, nationality n where t.manager_id = m.manager_id and n.nationality_id = m.nationality_id and t.team_id = '{cmbbox_manager.SelectedValue.ToString()}' and working = 1;";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtmanager);
            dataGridView1.DataSource = dtmanager;
            query = "select m.manager_name as 'Name', date_format(m.birthdate,'%Y-%m-%d') as 'Birthdate',n.nation from manager m, nationality n where n.nationality_id = m.nationality_id and m.working = '0';";
            cmd = new MySqlCommand(query, conn);
            adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(dtworking);
            dataGridView2.DataSource = dtworking;
        }

        private void cmbbox_selectedteam_SelectionChangeCommitted(object sender, EventArgs e)
        {
            updatedgv();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            int row = dataGridView3.RowCount;
            if (row < 11)
            {
                MessageBox.Show("Total player must be greater than 11");
            }
            else
            {
                query = $"update player set status = 0 where player_name = '{dataGridView3.SelectedCells[0].Value.ToString()}'";
                try
                {
                    conn.Open();
                    cmd = new MySqlCommand(query, conn);
                    reader = cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    updatedgv();
                }
            }
        }
    }
}
