using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace APP
{
    public class BancoDados
    {
        public SqlConnection cnn;

        public void DBConectar()
        {
            string connetionString = null;

            try
            {
                connetionString = "server=.\\SQLEXPRESS;database=Franquias_repro;uid=sa;pwd=832285";
                connetionString = "server=192.168.0.250\\SQLEXPRESS;database=Franquias_repro;uid=sa;pwd=832285";
                connetionString = APP.Properties.Settings.Default.connetionString;
                cnn = new SqlConnection(connetionString);
                cnn.Open();
            }
            catch (Exception ex)
            {
                try
                {
                    connetionString = "server=189.16.249.117\\SQLEXPRESS;database=Franquias_repro;uid=sa;pwd=832285";
                    cnn = new SqlConnection(connetionString);
                    cnn.Open();
                }
                catch (Exception ex1)
                {
                }
            }
        }

        public void DBDesconectar()
        {
            cnn.Close();
        }

        public object DBRetornarValorUnico(string sSqlText)
        {
            object Valor = null;

            SqlCommand cmd = new SqlCommand(sSqlText, cnn);

            //instância do leitor 
            SqlDataReader ret = cmd.ExecuteReader();

            //enquanto leitor lê 
            while (ret.Read())
            {
                Valor = ret[0].ToString();
            }

            ret.Close();
            ret = null;

            return Valor;
        }

        public int DBRetornarInt(string sSqlText)
        {
            int ID = 0;

            SqlCommand cmd = new SqlCommand(sSqlText, cnn);

            //instância do leitor 
            SqlDataReader ret = cmd.ExecuteReader();

            //enquanto leitor lê 
            while (ret.Read())
            {
                ID = Convert.ToInt32(ret[0].ToString());
            }

            ret.Close();
            ret = null;

            return ID;
        }

        public static SqlParameter DBParametro(string sNome,
                                               object oValor,
                                               System.Data.DbType Tipo = System.Data.DbType.String,
                                               int Tamanho = 0)
        {
            SqlParameter oParametro;
            oParametro = new SqlParameter();

            try
            {
                oParametro.ParameterName = sNome;
                oParametro.DbType = Tipo;
                oParametro.Value = oValor;
                if (Tamanho != 0) oParametro.Size = Tamanho;
            }
            catch (Exception Ex)
            {
            }

            return oParametro;
        }
    }
}