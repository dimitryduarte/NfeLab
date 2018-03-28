using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Oracle.DataAccess;
using Oracle.DataAccess.Client;

namespace NFeLab
{
    public partial class Form1 : Form
    {
        //OracleConnection conn = new OracleConnection("Data Source=dbserver;User Id=SYS;Password=123;DBA Privilege=SYSDBA");
        OracleConnection conn = new OracleConnection("Data Source=dbserver;User Id=dbuser;Password=123");
        DataTable dtb_itens = new DataTable();
        public Form1()
        {
            InitializeComponent();
            dtItens.Columns.Add("DESCRICAO", "DESCRICAO");
            dtItens.Columns.Add("VALORUN", "UNIT");
            dtItens.Columns.Add("QUANT", "QUANT");
            dtItens.Columns.Add("TOTALNOTA", "TOTAL");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //conn.ConnectionString = "Data Source = dbserver; User Id = dbuser; Password = 123";
            try {
                conn.Close();
                conn.Open();
                OracleCommand comm = new OracleCommand();
                comm.Connection = conn;
                comm.CommandText = "INSERT INTO NOTA(ID_NOTA,NAT_OPER,CLIENTE,DATA) VALUES((SELECT MAX(ID_NOTA)+1 FROM NOTA),'" + txbNatOper.Text+"','"+txbCliente.Text+"','"+txbData.Text+"')";
                comm.ExecuteNonQuery();
                //conn.Close();
            } catch (Exception ex) {
               // MessageBox.Show("NAO FOI POSSIVEL INSERIR A NOTA!");
                MessageBox.Show(ex.Message);              
            }
            
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            limpar();
        }

        private void adicionarNovoItemNaGrid()
        {
            string descricao;
            decimal valorun;
            int quant;

            descricao = txbDescItem.Text;
            valorun = Convert.ToDecimal(txbValorItem.Text);
            quant = Convert.ToInt32(txbQuantItem.Text);

            dtb_itens.Rows.Add(descricao, valorun, quant, valorun * quant);
            //dtItens.Rows.Add(descricao,valorun,quant,valorun*quant);
        }

         private DataTable exibirItensNota(int id_nota)
        {
                dtb_itens.Columns.Clear();
                dtb_itens.Rows.Clear();

                dtb_itens.Columns.Add("DESCRICAO", typeof(string));
                dtb_itens.Columns.Add("VALORUN", typeof(decimal));
                dtb_itens.Columns.Add("QUANT", typeof(int));
                dtb_itens.Columns.Add("TOTALNOTA", typeof(decimal));

            OracleDataReader dr = null;

            string descr = "";
            decimal valorun = 0;
            int quant = 0;
            decimal totalnota = 0;

            try
            {
                conn.Close();
                OracleCommand comm = new OracleCommand();
                conn.Open();
                comm.Connection = conn;
                comm.CommandText = "SELECT DESCRICAO,VALORUN,QUANT FROM ITEMNOTA WHERE FK_NOTA = '" + id_nota + "'";
                dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    descr = Convert.ToString(dr.GetString(0));
                    valorun = dr.GetDecimal(1);
                    quant = dr.GetInt32(2);
                    totalnota = valorun * quant;

                    dtb_itens.Rows.Add(descr,valorun,quant,totalnota);
                }
                
                conn.Close();
                return dtb_itens;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Não foi possível encontrar os itens da nota. Tente novamente!");
                MessageBox.Show(ex.Message);
                return dtb_itens;
            }
        }

        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            adicionarNovoItemNaGrid();
        }

        private void removerItemDaGrid()
        {
            dtItens.Rows.Remove(dtItens.CurrentRow);
        }

        private void btnRemoverItem_Click(object sender, EventArgs e)
        {
            removerItemDaGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            limpar();
            pesquisarNota(Convert.ToInt32(txbIdNota.Text));
            dtItens.DataSource = exibirItensNota(Convert.ToInt32(txbIdNota.Text));
        }

        private void pesquisarNota(int id_nota)
        {
            OracleDataReader dr = null;
            try
            {
                conn.Close();
                OracleCommand comm = new OracleCommand();
                conn.Open();
                comm.Connection = conn;
                comm.CommandText = "SELECT NAT_OPER,CLIENTE,DATA FROM NOTA WHERE ID_NOTA = '" + id_nota + "'";
                dr = comm.ExecuteReader();

                while (dr.Read())
                {
                    txbNatOper.Text = dr.GetString(0);
                    txbCliente.Text = Convert.ToString(dr.GetString(1));
                   // txbData.Text = Convert.ToString(dr.GetString(2));
                }
                dr.Close();
                conn.Close();
            }
            finally
            {
                // fecha o reader
                if (dr != null)
                {
                    dr.Close();
                }
                // 5. Fecha a conexão
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void limpar()
        {
            try
            {
                dtItens.Columns.Remove("DESCRICAO");
                dtItens.Columns.Remove("VALORUN");
                dtItens.Columns.Remove("QUANT");
                dtItens.Columns.Remove("TOTALNOTA");
            }
            catch (Exception ex) { }
            finally
            {
                txbCliente.Clear();
                txbDescItem.Clear();
                //txbIdNota.Clear();
                txbNatOper.Clear();
                txbQuantItem.Clear();
                txbValorItem.Clear();
                DataTable dtb_itens = new DataTable();
                dtb_itens.Columns.Clear();
                dtb_itens.Rows.Clear();
            }

        }
    }
}
