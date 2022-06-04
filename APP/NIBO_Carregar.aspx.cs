using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;

namespace APP
{
    public partial class NIBO_Carregar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void cmdProcessar_Click(object sender, EventArgs e)
        {
            string sMensagem = "";

            if (FileUploadControl.HasFile)
            {
                try
                {
                    BancoDados oDB = new BancoDados();
                    string sSqlText;
                    SqlCommand querySaveStaff;

                    int iCont = 0;
                    int iFilial = 0;
                    int iQuantidade = 0;
                    string filename = Path.GetFileName(FileUploadControl.FileName);

                    string sData = "";
                    string sRotulo = "";

                    FileUploadControl.SaveAs(Server.MapPath("~/") + filename);

                    filename = Server.MapPath("~/") + filename;

                    oDB.DBConectar();

                    try
                    {
                        var WK = new XLWorkbook(filename);
                        var WS = WK.Worksheet(1);
                        var Linha = 2;

                        SqlCommand cmd;
                        SqlConnection conn = new SqlConnection("server=192.168.0.250;database=Franquias_repro;uid=sa;pwd=832285");
                        conn.Open();

                        while (true)
                        {
                            var nome = WS.Cell("D" + Linha.ToString()).Value.ToString();

                            if (string.IsNullOrEmpty(nome)) break;

                            cmd = new SqlCommand("SP_NIBO_SOFTWARES_INS", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Prepare();
                            cmd.Parameters.Add("@CD_COMPETENCIA", SqlDbType.VarChar).Value = WS.Cell("A" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@DT_VENCIMENTO", SqlDbType.DateTime).Value = WS.Cell("B" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@NOME", SqlDbType.VarChar).Value = WS.Cell("C" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@DESCRICAO", SqlDbType.VarChar).Value = WS.Cell("D" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = WS.Cell("E" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@CATEGORIA", SqlDbType.VarChar).Value = WS.Cell("F" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@CENTRO_CUSTO", SqlDbType.VarChar).Value = WS.Cell("G" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@VALOR", SqlDbType.Float).Value = WS.Cell("H" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@PAGAMENTO", SqlDbType.DateTime).Value = WS.Cell("I" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@IDENTIFICADOR", SqlDbType.VarChar).Value = WS.Cell("J" + Linha.ToString()).Value.ToString();
                            cmd.Parameters.Add("@CONTA", SqlDbType.VarChar).Value = WS.Cell("K" + Linha.ToString()).Value.ToString();
                            cmd.ExecuteNonQuery();

                            Linha = Linha + 1;
                        }

                        cmd = new SqlCommand("SP_NIBO_SOFTWARES_PROCESSAR", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();

                        WK.Dispose();

                        try
                        {
                            sData = DateTime.Now.ToString();

                            if (System.IO.File.Exists(Server.MapPath(@"\Planilhas\" + FileUploadControl.FileName.Replace(".xlsx", "") + " " + sData.Replace("/", "-").Substring(0, 13) + ".xlsx")))
                            {
                                System.IO.File.Delete(Server.MapPath(@"\Planilhas\" + FileUploadControl.FileName.Replace(".xlsx", "") + " " + sData.Replace("/", "-").Substring(0, 13) + ".xlsx"));
                            }

                            System.IO.File.Move(Server.MapPath(@"\" + FileUploadControl.FileName),
                                                Server.MapPath(@"\Planilhas\" + FileUploadControl.FileName.Replace(".xlsx", "") + " " + sData.Replace("/", "-").Substring(0, 13) + ".xlsx"));
                        }
                        catch (Exception)
                        {
                        }

                        sMensagem = "Status do processamento: Planilha carregada!";
                    }
                    catch (Exception ex1)
                    {
                        sMensagem = "Erro ao processar o arquivo: " + ex1.Message;
                        goto Sair;
                    }
                }
                catch (Exception ex2)
                {
                    sMensagem = "Erro ao processar o arquivo: " + ex2.Message;
                    goto Sair;
                }
            }

        Sair:
            StatusLabel.Text = sMensagem;
        }
    }
}