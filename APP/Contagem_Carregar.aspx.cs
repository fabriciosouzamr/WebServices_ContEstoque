using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APP
{
    public partial class Contagem_Carregar : System.Web.UI.Page
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
                        StreamReader rd = new StreamReader(@filename);
                        string sFilial = "";
                        string sCD_Produto = "";

                        string linha = null;
                        string[] linhaseparada = null;

                        while ((linha = rd.ReadLine()) != null)
                        {
                            if (iCont > 0)
                            {
                                linhaseparada = linha.Split(';');

                                sFilial = linhaseparada[0];

                                if (linhaseparada[1].Trim() == "")
                                {
                                    sMensagem = "Erro ao processar o arquivo: Não foi informado o código para todos os produtos";
                                    goto Sair;
                                }

                                if (iFilial == 0)
                                {
                                    sSqlText = "SELECT ID_FILIAL FROM TB_FILIAL WHERE UPPER(DS_REDUZIDO) = '" + sFilial.Trim().ToUpper() + "'";
                                    iFilial = oDB.DBRetornarInt(sSqlText);

                                    if (iFilial == 0)
                                    {
                                        sMensagem = "Erro ao processar o arquivo: Filial não encontrada (" + sFilial + ")";
                                        goto Sair;
                                    }
                                    else
                                    {
                                        sSqlText = "DELETE FROM TB_TMP_COD_SALDO WHERE ID_FILIAL = " + iFilial.ToString();
                                        querySaveStaff = new SqlCommand(sSqlText, oDB.cnn);
                                        querySaveStaff.Prepare();
                                        querySaveStaff.ExecuteNonQuery();
                                    }
                                }

                                if (optPesquisaProduto.SelectedValue.Trim() == "1")
                                {
                                    sCD_Produto = linhaseparada[1];
                                }
                                else if (optPesquisaProduto.SelectedValue.Trim() == "2")
                                {
                                    sSqlText = "SELECT CD_PRODUTO FROM TB_PRODUTO WHERE RTRIM(LTRIM(CD_CODIGOBARRA)) = '" + linhaseparada[1] + "'";
                                    sCD_Produto = oDB.DBRetornarValorUnico(sSqlText).ToString();
                                }

                                sSqlText = "INSERT INTO TB_TMP_COD_SALDO (CD_PRODUTO, QT_SALDO, ID_FILIAL) VALUES (@CD_PRODUTO, @QT_SALDO, @ID_FILIAL)";

                                try
                                {
                                    iQuantidade = Convert.ToInt32(linhaseparada[4]);
                                }
                                catch (Exception)
                                {
                                    iQuantidade = 0;
                                }

                                querySaveStaff = new SqlCommand(sSqlText, oDB.cnn);
                                querySaveStaff.Prepare();
                                querySaveStaff.Parameters.Add(BancoDados.DBParametro("CD_PRODUTO", sCD_Produto));
                                querySaveStaff.Parameters.Add(BancoDados.DBParametro("QT_SALDO", iQuantidade));
                                querySaveStaff.Parameters.Add(BancoDados.DBParametro("ID_FILIAL", iFilial));

                                try
                                {
                                    querySaveStaff.ExecuteNonQuery();
                                }
                                catch (SqlException ex)
                                {
                                }
                            }

                            iCont = iCont + 1;
                        }
                        rd.Close();

                        if (optContagemRealizada.SelectedValue.Trim() == "1")
                        {
                            sData = Convert.ToDateTime(txtData.Text).AddDays(-1).Date.ToString().Substring(0, 10) + " 23:00:00.000";
                            sRotulo = "SLD " + Convert.ToDateTime(txtData.Text).ToString().Substring(0, 10);
                        }
                        else if (optContagemRealizada.SelectedValue.Trim() == "2")
                        {
                            sData = Convert.ToDateTime(txtData.Text).ToString().Substring(0, 10) + " 23:00:00.000";
                            sRotulo = "SLD " + Convert.ToDateTime(txtData.Text).AddDays(1).ToString().Substring(0, 10);
                        }

                        sSqlText = "SP_CONTAGEMESTOQUE_UPD";
                        querySaveStaff = new SqlCommand(sSqlText, oDB.cnn);
                        querySaveStaff.CommandType = System.Data.CommandType.StoredProcedure;
                        querySaveStaff.Prepare();
                        querySaveStaff.Parameters.Add(BancoDados.DBParametro("IFILIAL", iFilial));
                        querySaveStaff.Parameters.Add(BancoDados.DBParametro("PERIODO", sData));
                        querySaveStaff.Parameters.Add(BancoDados.DBParametro("CMLANCAMENTO", sRotulo));

                        querySaveStaff.CommandTimeout = 0;
                        querySaveStaff.ExecuteNonQuery();

                        if (System.IO.File.Exists(Server.MapPath(@"\Planilhas\" + FileUploadControl.FileName.Replace(".csv", "") + " " + sData.Replace("/", "-").Substring(0, 13) + ".csv")))
                        {
                            System.IO.File.Delete(Server.MapPath(@"\Planilhas\" + FileUploadControl.FileName.Replace(".csv", "") + " " + sData.Replace("/", "-").Substring(0, 13) + ".csv"));
                        }

                        System.IO.File.Move(Server.MapPath(@"\" + FileUploadControl.FileName), 
                                            Server.MapPath(@"\Planilhas\" + FileUploadControl.FileName.Replace(".csv", "") + " " + sData.Replace("/", "-").Substring(0, 13) + ".csv"));

                        sMensagem = "Status do processamento: Estoque atualizado!";
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