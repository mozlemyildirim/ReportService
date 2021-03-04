using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using VeraDAL.Entities;

namespace VeraReportService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            while (true)
            {
                var dt = DateTime.Now;
                if (dt.Hour == 00 && dt.Minute == 00) //(true)
                {
                    using (var context = new VeraEntities())
                    {
                        var usersToSendReports = context.UserReport.Where(x => x.AutomaticReportingStatus == 1).Select(x => x.UserId).Distinct().ToList();
                        
                        foreach (var userId in usersToSendReports)
                        {
                            var user = context.User.FirstOrDefault(x => x.Id == userId);

                            if (user == null)
                            {
                                continue;
                            }

                            if (user.Mail == null || user.Mail.Trim() == "" || !user.Mail.Contains('@'))
                            {
                                continue;
                            }
                            var reportIds = context.UserReport.Where(x => x.UserId == userId && x.AutomaticReportingStatus == 1).Select(x => x.ReportId).ToList();
                            if (reportIds.Count == 0)
                            {
                                continue;
                            }
                            var reports = context.Report.Where(x => reportIds.Contains(x.Id)).ToList();
                            foreach (var item in reports)
                            {
                                var userReport = context.UserReport.FirstOrDefault(x => x.ReportId == item.Id && x.UserId == user.Id);
                                //var excelOrPdf = userReport.UserReportType == 1 ? 2 : 1;
                                var startDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                                var endDate = DateTime.Now.ToString("yyyy-MM-dd");
                                var userReportVehicles = userReport.AutomaticReportingVehicles;
                                var userReportFileType = userReport.UserReportType;
                                // gun = 2019-09-27  - 2019-09-27
                                // Tetiklenme 2019 -09-28:00
                                if (item.Id == 1)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportSeferOzetiRaporu?_startDateSOR={startDate}&_endDateSOR={endDate}&_vehiclesSOR={userReportVehicles}&_fileTypeSOR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={userReport.UserId}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)   
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }
                                /*if (item.Id == 2)
                                {
                                    //SeferOlay
                                    try{
                                    var url = $"http://veraadmin.intellity-ai.com/Email/ExportSeferOlayRaporu?_startDateSOR={startDate}&_endDateGKR={endDate}&_vehiclesSOR={userReportVehicles}&_fileTypeSOR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                    HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                    if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                        Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                    myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }*/
                                if (item.Id == 3)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportGecmisKonumRaporu?_startDateGKR={startDate}&_endDateGKR={endDate}&_vehiclesGKR={userReportVehicles}&_fileTypeGKR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }
                                if (item.Id == 4)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportSonKonumRaporu?_groupIdSKR=0&_fileTypeSKR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }
                                if (item.Id == 5)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportGiseGecisRaporu?_startDateGGR={startDate}&_endDateGGR={endDate}&_vehiclesGGR={userReportVehicles}&_fileTypeGGR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }
                                if (item.Id == 6)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportBolgeVarisAyrilisRaporu?_startDateBVAR={startDate}&_endDateBVAR={endDate}&_companyAreaBVAR=0&_vehiclesBVAR={userReportVehicles}&_fileTypeBVAR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }
                                if (item.Id == 7)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportMesafeOzetRaporu?_startDateMOR={startDate}&_endDateMOR={endDate}&_vehiclesMOR={userReportVehicles}&_fileTypeMOR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }
                                if (item.Id == 8)
                                {
                                    try
                                    {
                                        var url = $"http://veraadmin.intellity-ai.com/Email/ExportKontakDurumRaporu?_startDateKDR={startDate}&_endDateKDR={endDate}&_vehiclesKDR={userReportVehicles}&_fileTypeKDR={userReportFileType}&_downloadOrMail=2&_emailToSendReport={user.Mail}&_userId={user.Id}";
                                        HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                                        if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                                            //Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}", myHttpWebResponse.StatusDescription);
                                        myHttpWebResponse.Close();
                                    }
                                    catch (WebException exc)
                                    {
                                        //Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", exc.Status);
                                    }
                                    catch (Exception exc)
                                    {
                                        //Console.WriteLine("\nThe following Exception was raised : {0}", exc.Message);
                                    }
                                }


                            }

                        }
                    }

                    Thread.Sleep(60000);
                    //Console.WriteLine("Mailler Gönderildi!");
                }
            }
        }

        protected override void OnStop()
        {

        }
        
    }
}
