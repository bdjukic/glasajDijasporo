using GlasajDijasporoService.Helpers;
using GlasajDijasporoService.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace GlasajDijasporoService.Controllers
{
    public class ServiceController : ApiController
    {
        // POST api/service
        [HttpPost]
        public HttpResponseMessage Post([FromBody]VotingRequest votingRequest)
        {
            if (votingRequest == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var contentFolder = HttpContext.Current.Server.MapPath("~/Content");
            var pdfFileTemplate = contentFolder + @"/VotingRequestDocument.pdf";

            using (var existingFileStream = new FileStream(pdfFileTemplate, FileMode.Open))
            {
                using (var newPdfFileStream = new MemoryStream())
                {
                    var xCord = 296;
                    var yCord = 615;

                    var pdfReader = new PdfReader(existingFileStream);
                    var stamper = new PdfStamper(pdfReader, newPdfFileStream);
                    var content = stamper.GetOverContent(1);

                    content.BeginText();
                    content.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false), 14f);

                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.FirstLastName, xCord, yCord, 0);

                    yCord -= 23;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.BirthPlaceDate, xCord, yCord, 0);

                    yCord -= 24;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.Gender, xCord, yCord, 0);

                    yCord -= 22;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.ParentName, xCord, yCord, 0);

                    yCord -= 22;
                    float tempXCoord = xCord + 5;
                    if (votingRequest.PersonalId != null)
                    {
                        foreach (char number in votingRequest.PersonalId)
                        {
                            content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, number.ToString(), tempXCoord, yCord, 0);
                            tempXCoord += 17.5f;
                        }
                    }

                    yCord -= 28;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.PassportId, xCord, yCord, 0);

                    yCord -= 35;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.HomeCountryAddress, xCord, yCord, 0);

                    yCord -= 38;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.TemporaryAddress, xCord, yCord, 0);

                    yCord -= 35;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.ForeignCountryName, xCord, yCord, 0);

                    yCord -= 38;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.ForeignCountryAddress, xCord, yCord, 0);

                    yCord -= 48;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.VotingLocation, xCord, yCord, 0);

                    yCord -= 40;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.CurrentLocation, xCord - 163, yCord, 0);
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, DateTime.Now.ToString("dd. MM. yyyy."), xCord - 60, yCord, 0);

                    yCord -= 55;
                    var bytes = Convert.FromBase64String(votingRequest.Signature.Split(',')[1]);
                    using (var rawImage = new MemoryStream(bytes))
                    {
                        Image image = Image.GetInstance(ImageHelper.ScaleImage(rawImage, 150, 150), ImageFormat.Png);

                        image.SetAbsolutePosition(xCord + 85, yCord);
                        content.AddImage(image);
                    }

                    content.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false), 12f);

                    yCord -= 23;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.PhoneNumber, xCord + 50, yCord, 0);
                    yCord -= 12;
                    content.ShowTextAligned(PdfContentByte.ALIGN_LEFT, votingRequest.Email, xCord + 50, yCord, 0);

                    content.EndText();

                    stamper.Close();
                    pdfReader.Close();

                    response.Content = new ByteArrayContent(newPdfFileStream.GetBuffer());
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = "Zahtev.pdf";
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                }
            }

            return response;    
        }
    }
}
