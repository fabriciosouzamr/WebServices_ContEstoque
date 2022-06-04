using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Services;

namespace WS
{
    /// <summary>
    /// Descrição resumida de Certificado
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    // [System.Web.Script.Services.ScriptService]
    public class Certificado : System.Web.Services.WebService
    {
        [WebMethod]
        public string Instalar(String CaminhoCertificado, String Senha)
        {
            string fileName = Server.MapPath("~") + "/UploadedFiles/arq.txt";

            using (HttpClient client = new HttpClient())
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            using (FileStream fileStream = System.IO.File.OpenRead(fileName))
            using (StreamContent fileContent = new StreamContent(fileStream))
            {
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("text/plain");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = "arq.txt",
                };
                fileContent.Headers.Add("name", "arq.txt");

                content.Add(fileContent);
                var result = client.PostAsync("http://localhost:62951/api/arquivo", content).Result;
                result.EnsureSuccessStatusCode();

                return "Olá, Mundo";
            }
        }
    }
}