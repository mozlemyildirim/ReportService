using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class ReportDB
    {
        private static ReportDB instance;
        public static ReportDB GetInstance()
        {
            if (instance == null)
            {
                instance = new ReportDB();
            }
            return instance;
        }
        public Report AddNewReport(Report _Report)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Report.Add(_Report);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Report : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Report> GetAllReportes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var ReportList = context.Report.ToList();
                    return ReportList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Report GetReportById(int _ReportId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Report = context.Report.FirstOrDefault(a => a.Id == _ReportId && a.Status == 1);
                    return Report != null ? Report : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteReport(int _ReportId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Report = context.Report.FirstOrDefault(a => a.Id == _ReportId);
                    if (Report != null)
                    {

                        Report.Status = 0;
                        int numOfDeleted = context.SaveChanges();
                        return numOfDeleted > 0;
                    }
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void UpdateObject(Report _newReport, ref Report _oldReport)
        {
            try
            {

                foreach (PropertyInfo ReportPropInfo in _newReport.GetType().GetProperties().ToList())
                {
                    _oldReport.GetType().GetProperty(ReportPropInfo.Name).SetValue(_oldReport, _newReport.GetType().GetProperty(ReportPropInfo.Name).GetValue(_newReport));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Report UpdateReport(Report _Report)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldReport = context.Report.FirstOrDefault(u => u.Id == _Report.Id);
                    if (oldReport != null)
                    {
                        UpdateObject(_Report, ref oldReport);
                        var numberOfUpdatedReport = context.SaveChanges();
                        return numberOfUpdatedReport > 0 ? _Report : null;
                    }

                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Dictionary<SORKeyClass, List<SeferOzetiRaporuObjectRepo>> SeferOzetiRaporu(string _startDateSOR, string _endDateSOR, string[] _vehiclesSOR)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {

                            if (_startDateSOR.Length > 11 && _endDateSOR.Length > 11)
                            {
                                _startDateSOR = DateTime.ParseExact(_startDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                _endDateSOR = DateTime.ParseExact(_endDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (_startDateSOR.Length < 11 && _endDateSOR.Length < 11)
                            {
                                _startDateSOR += " 00:00:00";
                                _endDateSOR += " 23:59:59";
                                _startDateSOR = Convert.ToDateTime(_startDateSOR).ToString("dd.MM.yyyy HH:mm:ss");
                                _endDateSOR = Convert.ToDateTime(_endDateSOR).ToString("dd.MM.yyyy HH:mm:ss");
                                _startDateSOR = DateTime.ParseExact(_startDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                _endDateSOR = DateTime.ParseExact(_endDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            //var startDate = DateTime.ParseExact(_startDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            // var endDate = DateTime.ParseExact(_endDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");


                            var list1 = new List<SeferOzetiRaporuObjectRepo>();
                            string[] deviceIds = _vehiclesSOR[0].Split(',');
                            var dic = new Dictionary<SORKeyClass, List<SeferOzetiRaporuObjectRepo>>();
                            for (int i = 0; i < deviceIds.Length; i++)
                            {
                                list1 = new List<SeferOzetiRaporuObjectRepo>();
                                var _deviceId = Convert.ToInt32(deviceIds[i]);
                                var device = context.Device.FirstOrDefault(x => x.Id == _deviceId);

                                var groupDevice = context.GroupDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                                var grupName = "Grupsuz";
                                if (groupDevice != null)
                                {
                                    grupName = context.Group.FirstOrDefault(x => x.Id == groupDevice.GroupId).Name;
                                }
                                var keyClass = new SORKeyClass()
                                {
                                    MobilTanimi = device.CarPlateNumber,
                                    Grup = grupName
                                };

                                if (device.LastDeviceDataId == null)
                                {
                                    var emptyObject = new SeferOzetiRaporuObjectRepo();
                                    var emptyList = new List<SeferOzetiRaporuObjectRepo>();
                                    emptyList.Add(emptyObject);
                                    dic.Add(keyClass, emptyList);
                                    continue;
                                }



                                cmd.CommandText = $"EXEC USP_SeferOzetiRaporu '{_startDateSOR }','{_endDateSOR }',{_deviceId}";


                                var reader = cmd.ExecuteReader();

                                StringBuilder sb = new StringBuilder();

                                while (reader.Read())
                                    sb.Append(reader.GetString(0));

                                reader.Close();

                                var json = sb.ToString();
                                var list2 = JsonConvert.DeserializeObject<List<SeferOzetiRaporuObject>>(json);
                                list2 = list2.Where(x => x.EndKontakAcilmaTarihi != null).ToList();
                                if (list2.Count == 0 || list2 == null)
                                {
                                    var emptyList = new List<SeferOzetiRaporuObjectRepo>();
                                    dic.Add(keyClass, emptyList);
                                    continue;
                                }


                                //var returnList = new List<SeferOzetiRaporuObjectRepo>();
                                //var tempDeviceDatas = context.DeviceData.Where(x => x.DeviceId == _deviceId && x.CreateDate >= Convert.ToDateTime(_startDateSOR) && x.CreateDate <= Convert.ToDateTime(_endDateSOR)).OrderBy(x => x.Id).ToList();

                                //var IoStatusesAllOfThem1 = tempDeviceDatas.Where(x => x.IoStatus.EndsWith("1")).Select(x=>x.IoStatus).ToList();
                                //var IoStatusesAllOfThem0 = tempDeviceDatas.Where(x => x.IoStatus.EndsWith("0")).Select(x=>x.IoStatus).ToList();
                                //if (IoStatusesAllOfThem1.Count == tempDeviceDatas.Count || IoStatusesAllOfThem0.Count == tempDeviceDatas.Count)
                                //{
                                //    var emptyObject = new SeferOzetiRaporuObjectRepo();
                                //    var emptyList = new List<SeferOzetiRaporuObjectRepo>();
                                //    emptyList.Add(emptyObject);
                                //    dic.Add(keyClass, emptyList);
                                //    continue;
                                //}
                                //var emptyDeviceData = new DeviceData();
                                //tempDeviceDatas.Insert(0, emptyDeviceData); 

                                //tempDeviceDatas[0] = null;


                                //for (int b = 1; b <= tempDeviceDatas.Count; b++)
                                //{
                                //    if (b == tempDeviceDatas.Count)
                                //        break;
                                //    var item = tempDeviceDatas[b];

                                //    if (item.IoStatus.EndsWith("0"))
                                //    {
                                //        if (tempDeviceDatas[b - 1] == null || tempDeviceDatas[b - 1].IoStatus.EndsWith("0"))
                                //            continue;
                                //    }
                                //    if (item.IoStatus.EndsWith("1"))//Start
                                //    {
                                //        if (tempDeviceDatas[b - 1] == null)
                                //        {
                                //            returnList.Add(new SeferOzetiRaporuObjectRepo()
                                //            {
                                //                StartLat = item.Latitude.ToString(),
                                //                StartLong = item.Longtitude.ToString(),
                                //                StartKontakAcilmaTarihi = item.CreateDate.ToString(),
                                //                StartLocation = GetLocationFromLatLon(item.Latitude, item.Longtitude),
                                //                StartTotalKm = item.TotalKm.ToString(),
                                //                StartDeviceDataId = item.Id.ToString()
                                //            });
                                //            continue;
                                //        }
                                //    }
                                //    if (tempDeviceDatas[b - 1] != null)
                                //    {
                                //        if (tempDeviceDatas[b - 1].IoStatus.EndsWith("1") && item.IoStatus.EndsWith("0"))
                                //        { //End 
                                //            var tripStart = returnList.FirstOrDefault(x => x.EndDeviceDataId == null);
                                //            var averageAndMaxKm = GetAverageAndMaxKmFromDeviceDataList(device.Id, Convert.ToDateTime(tripStart.StartKontakAcilmaTarihi).ToString("yyyy-MM-dd HH:mm:ss"), item.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));

                                //            returnList.Add(new SeferOzetiRaporuObjectRepo()
                                //            {
                                //                StartLat = tripStart.StartLat,
                                //                StartLong = tripStart.StartLong,
                                //                StartKontakAcilmaTarihi = tripStart.StartKontakAcilmaTarihi,
                                //                StartLocation = tripStart.StartLocation,
                                //                StartTotalKm = tripStart.StartTotalKm,
                                //                StartDeviceDataId = tripStart.StartDeviceDataId,
                                //                EndLat = item.Latitude.ToString(),
                                //                EndLong = item.Longtitude.ToString(),
                                //                EndKontakAcilmaTarihi = item.CreateDate.ToString(),
                                //                EndTotalKm = item.TotalKm.ToString(),
                                //                EndDeviceDataId = item.Id.ToString(),
                                //                Mesafe = (item.TotalKm - Convert.ToInt32(tripStart.StartTotalKm)).ToString(),
                                //                Arac = device.CarPlateNumber,
                                //                KalkisZamani = tripStart.StartKontakAcilmaTarihi,
                                //                VarisZamani = item.CreateDate.ToString(),
                                //                KalkisPozisyon = tripStart.StartLocation,
                                //                VarisPozisyon = GetLocationFromLatLon(item.Latitude, item.Longtitude),
                                //                ToplamKm = item.TotalKm.ToString(),
                                //                Sure = TotalHoursToHumanReadableString((item.CreateDate - Convert.ToDateTime(tripStart.StartKontakAcilmaTarihi)).TotalHours.ToString()),
                                //                OrtHiz = averageAndMaxKm[0].AverageKm/*tempDeviceDatas.Where(x => x.CreateDate > Convert.ToDateTime(tripStart.StartKontakAcilmaTarihi) && x.CreateDate < item.CreateDate).Average(x => x.KmPerHour).ToString()*/,
                                //                MaxHiz = averageAndMaxKm[0].MaxKm/*tempDeviceDatas.Where(x => x.CreateDate > Convert.ToDateTime(tripStart.StartKontakAcilmaTarihi) && x.CreateDate < item.CreateDate).Max(x => x.KmPerHour).ToString()*/ 
                                //            });
                                //            returnList.Remove(tripStart);
                                //            continue;
                                //        }
                                //        if (tempDeviceDatas[b - 1] != null)
                                //        {
                                //            if (tempDeviceDatas[b - 1].IoStatus.EndsWith("0") && item.IoStatus.EndsWith("1"))
                                //            { //The other start
                                //                returnList.Add(new SeferOzetiRaporuObjectRepo()
                                //                {
                                //                    StartLat = item.Latitude.ToString(),
                                //                    StartLong = item.Longtitude.ToString(),
                                //                    StartKontakAcilmaTarihi = item.CreateDate.ToString(),
                                //                    StartLocation = GetLocationFromLatLon(item.Latitude, item.Longtitude),
                                //                    StartTotalKm = item.TotalKm.ToString(),
                                //                    StartDeviceDataId = item.Id.ToString() 
                                //                });
                                //                continue;
                                //            }
                                //        } 
                                //    } 
                                //}
                                foreach (var item in list2)
                                {
                                    list1.Add(new SeferOzetiRaporuObjectRepo
                                    {
                                        Arac = device.CarPlateNumber,
                                        KalkisZamani = item.StartKontakAcilmaTarihi == null ? "" : item.StartKontakAcilmaTarihi.Replace("T", " "),
                                        VarisZamani = item.EndKontakAcilmaTarihi == null ? "" : item.EndKontakAcilmaTarihi.Replace("T", " "),
                                        KalkisPozisyon = item.StartLocation,
                                        Sure = item.EndKontakAcilmaTarihi == null || item.StartKontakAcilmaTarihi == null ? "" : TotalHoursToHumanReadableString(Convert.ToString((Convert.ToDateTime(item.EndKontakAcilmaTarihi.Replace("T", " ")) - Convert.ToDateTime(item.StartKontakAcilmaTarihi.Replace("T", " "))).TotalHours)),
                                        Mesafe = item.EndTotalKm == null || item.StartTotalKm == null ? "" : Convert.ToString(Convert.ToInt32(item.EndTotalKm) - Convert.ToInt32(item.StartTotalKm)),
                                        ToplamKm = item.EndTotalKm == null ? "" : item.EndTotalKm,
                                        VarisPozisyon = item.EndLocation == null ? "" : item.EndLocation /*item.EndLat == null && item.EndLat == null ? "" : DeviceDB.GetInstance().GetLocationFromLatLon2(item.EndLat, item.EndLong)*/
                                    });
                                }
                                var startOfTheList = list2.FirstOrDefault();
                                var endOfTheList = list2.LastOrDefault();
                                keyClass.GecenSure = TotalHoursToHumanReadableString((Convert.ToDateTime(endOfTheList.EndKontakAcilmaTarihi) - Convert.ToDateTime(startOfTheList.StartKontakAcilmaTarihi)).TotalHours.ToString());
                                var averageAndMaxKm = GetAverageAndMaxKmFromDeviceDataList(device.Id, _startDateSOR, _endDateSOR);
                                keyClass.AverageKm = averageAndMaxKm[0].AverageKm;
                                keyClass.MaxKm = averageAndMaxKm[0].MaxKm;
                                dic.Add(keyClass, list1);
                            }
                            return dic;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
        public Dictionary<GKRKeyClass, List<GecmisKonumRaporuObjectRepo>> GecmisKonumRaporu(string _startDateGKR, string _endDateGKR, string[] _vehiclesGKR)
        {
            using (var context = new VeraEntities())
            {

                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {


                            if (_startDateGKR.Length > 11 && _endDateGKR.Length > 11)
                            {
                                _startDateGKR = DateTime.ParseExact(_startDateGKR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                _endDateGKR = DateTime.ParseExact(_endDateGKR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (_startDateGKR.Length < 11 && _endDateGKR.Length < 11)
                            {
                                _startDateGKR += " 00:00:00";
                                _endDateGKR += " 23:59:59";
                                _startDateGKR = Convert.ToDateTime(_startDateGKR).ToString("dd.MM.yyyy HH:mm:ss");
                                _endDateGKR = Convert.ToDateTime(_endDateGKR).ToString("dd.MM.yyyy HH:mm:ss");
                                _startDateGKR = DateTime.ParseExact(_startDateGKR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                _endDateGKR = DateTime.ParseExact(_endDateGKR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            var list1 = new List<GecmisKonumRaporuObjectRepo>();
                            string[] deviceIds = _vehiclesGKR[0].Split(',');

                            var dic = new Dictionary<GKRKeyClass, List<GecmisKonumRaporuObjectRepo>>();


                            for (int i = 0; i < deviceIds.Length; i++)
                            {
                                list1 = new List<GecmisKonumRaporuObjectRepo>();
                                var _deviceId = Convert.ToInt32(deviceIds[i]);
                                var device = context.Device.FirstOrDefault(x => x.Id == _deviceId);


                                var groupDevice = context.GroupDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                                var grupName = "Grupsuz";
                                if (groupDevice != null)
                                {
                                    grupName = context.Group.FirstOrDefault(x => x.Id == groupDevice.GroupId).Name;
                                }
                                var keyClass = new GKRKeyClass()
                                {
                                    MobilTanimi = device.CarPlateNumber,
                                    Grup = grupName
                                };

                                if (device.LastDeviceDataId == null)
                                {
                                    var emptyObject = new GecmisKonumRaporuObjectRepo();
                                    list1.Add(emptyObject);
                                    dic.Add(keyClass, list1);
                                    continue;
                                }


                                //var startDate = DateTime.ParseExact(_startDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss"); 
                                //var endDate = DateTime.ParseExact(_endDateSOR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                cmd.CommandText = $"EXEC USP_GecmisKonumRaporu '{_startDateGKR }','{_endDateGKR }',{_deviceId}";

                                var reader = cmd.ExecuteReader();

                                StringBuilder sb = new StringBuilder();

                                while (reader.Read())
                                    sb.Append(reader.GetString(0));

                                var json = sb.ToString();
                                var list2 = JsonConvert.DeserializeObject<List<GecmisKonumRaporuObject>>(json);

                                if (list2 == null || list2.Count == 0)
                                {
                                    var emptyObject = new GecmisKonumRaporuObjectRepo();
                                    list1.Add(emptyObject);
                                    dic.Add(keyClass, list1);
                                    continue;

                                }





                                var list3 = new List<GecmisKonumRaporuObject>();
                                list2 = list2.Where(x => x.Boylam != null).ToList();
                                // 1 dakikalık veri için
                                foreach (var item in list2)
                                {
                                    item.Tarih = Convert.ToDateTime(item.Tarih).ToString("yyyy-MM-dd hh:mm:00");
                                }
                                foreach (var item in list2)
                                {
                                    if (list3.Count > 0)
                                    {
                                        if (list3.LastOrDefault().Tarih == item.Tarih)
                                            continue;
                                    }
                                    list3.Add(item);
                                }
                                //

                                foreach (var item in list3)
                                {
                                    list1.Add(new GecmisKonumRaporuObjectRepo
                                    {
                                        Arac = item.Arac,
                                        Boylam = item.Boylam,
                                        Enlem = item.Enlem,
                                        Grup = item.Grup,
                                        Hiz = item.Hiz,
                                        Mesafe = item.Mesafe,
                                        Tarih = item.Tarih,
                                        ToplamKm = item.ToplamKm,
                                        Konum = item.Konum
                                    });
                                }

                                dic.Add(keyClass, list1);

                                reader.Close();
                            }
                            return dic;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
        public Dictionary<string, List<SonKonumRaporuObjectRepo>> SonKonumRaporu(int _groupIdSKR, User userObj)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {
                            var dic = new Dictionary<string, List<SonKonumRaporuObjectRepo>>();

                            var returnList = new List<SonKonumRaporuObjectRepo>();
                            if (_groupIdSKR != 0)
                            {
                                var groupHasDevice = context.GroupDevice.FirstOrDefault(x => x.GroupId == _groupIdSKR) == null ? false : true;
                                if (!groupHasDevice)
                                {
                                    return null;
                                }
                            }
                            cmd.CommandText = $"EXEC USP_SonKonumRaporu {_groupIdSKR},{userObj.Id}";
                            var reader = cmd.ExecuteReader();
                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));
                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<SonKonumRaporuObject>>(json);
                            reader.Close();
                            if (_groupIdSKR == 0) //hepsi seçiliyse
                            {
                                var groups = list.Select(x => x.Grup).Distinct().ToList();

                                for (int i = 0; i < groups.Count; i++)
                                {
                                    returnList = new List<SonKonumRaporuObjectRepo>();
                                    var list2 = list.Where(x => x.Grup.Equals(groups[i])).ToList();
                                    var group = groups[i];

                                    foreach (var item in list2)
                                    {
                                        returnList.Add(new SonKonumRaporuObjectRepo()
                                        {
                                            Grup = item.Grup,
                                            Konum = item.Konum,
                                            MobilTanimi = item.MobilTanimi,
                                            Tarih = Convert.ToDateTime(item.Tarih).ToString("yyyy-MM-dd HH:mm:ss"),
                                            ToplamKm = item.ToplamKm
                                        });
                                    }
                                    dic.Add(group, returnList);
                                }
                            }

                            if (_groupIdSKR != 0)
                            { // belirli bir grup için
                                var group = context.Group.First(x => x.Id == _groupIdSKR);
                                foreach (var item in list)
                                {
                                    returnList.Add(new SonKonumRaporuObjectRepo()
                                    {
                                        Grup = item.Grup,
                                        Konum = item.Konum,
                                        MobilTanimi = item.MobilTanimi,
                                        Tarih = Convert.ToDateTime(item.Tarih).ToString("yyyy-MM-dd HH:mm:ss"),
                                        ToplamKm = item.ToplamKm
                                    });
                                }
                                dic.Add(group.Name, returnList);

                            }
                            return dic;


                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }

        }
        public Dictionary<MORKeyClass, List<MesafeOzetRaporuObjectRepo>> MesafeOzetRaporu(string _startDateMOR, string _endDateMOR, string[] _vehiclesMOR, User _userObj)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {
                            var list1 = new List<MesafeOzetRaporuObjectRepo>();
                            string[] deviceIds = _vehiclesMOR[0].Split(',');

                            var dic = new Dictionary<MORKeyClass, List<MesafeOzetRaporuObjectRepo>>();


                            for (int i = 0; i < deviceIds.Length; i++)
                            {
                                list1 = new List<MesafeOzetRaporuObjectRepo>();
                                var _deviceId = Convert.ToInt32(deviceIds[i]);
                                var device = context.Device.FirstOrDefault(x => x.Id == _deviceId);


                                Area area = null;
                                var deviceArea = context.AreaDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                                if (deviceArea != null)
                                    area = context.Area.FirstOrDefault(x => x.Id == deviceArea.AreaId); 

                                var groupDevice = context.GroupDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                                var grupName = "Grupsuz";
                                if (groupDevice != null)
                                {
                                    grupName = context.Group.FirstOrDefault(x => x.Id == groupDevice.GroupId).Name;
                                }
                                var keyClass = new MORKeyClass()
                                {
                                    MobilTanimi = device.CarPlateNumber,
                                    Grup = grupName,
                                    Bolge=area==null?"-":area.Name
                                };

                                if (device.LastDeviceDataId == null)
                                {
                                    var emptyObject = new MesafeOzetRaporuObjectRepo();
                                    list1.Add(emptyObject);
                                    dic.Add(keyClass, list1);
                                    continue;
                                }

                                var company = context.Company.FirstOrDefault(x => x.Id == device.CompanyId);
                                var companyShift = context.CompanyShift.FirstOrDefault(x => x.CompanyId == company.Id && x.Status == 1);
                                Shift shift = null;
                                if (companyShift != null)
                                {
                                    shift = context.Shift.FirstOrDefault(x => x.Id == companyShift.Id && x.Status == 1);
                                }
                                var shiftStart = 0;
                                var shiftEnd = 0;
                                if (shift != null)
                                {
                                    shiftStart = Convert.ToInt32(shift.StartHour.Replace("0", "").Replace(":", ""));
                                    shiftEnd = Convert.ToInt32(shift.EndHour.Replace("0", "").Replace(":", ""));
                                }
                                cmd.CommandText = $"EXEC USP_MesafeOzetiRaporu '{_startDateMOR + " 00:00:00"}','{_endDateMOR + " 23:59:59"}',{_deviceId}";
                                var reader = cmd.ExecuteReader();
                                StringBuilder sb = new StringBuilder();
                                while (reader.Read())
                                    sb.Append(reader.GetString(0));
                                var json = sb.ToString(); 
                                reader.Close();
                                var list2 = JsonConvert.DeserializeObject<List<MesafeOzetRaporuObject>>(json);
                                if (list2.Count == 0 || list2 == null)
                                    continue;


                                

                                var daysCount = Convert.ToInt32((Convert.ToDateTime(_endDateMOR) - Convert.ToDateTime(_startDateMOR)).TotalDays);
                                if (daysCount == 0)
                                    daysCount = 1;
                                if (_startDateMOR == _endDateMOR)
                                {
                                    _endDateMOR = Convert.ToDateTime(_endDateMOR).AddDays(1).ToString("yyyy-MM-dd").Substring(0, 10);
                                }
                                for (int a = 0; a < daysCount; a++)
                                {
                                    var list = list2.Where(x => Convert.ToDateTime(x.CreateDate) >= Convert.ToDateTime(_startDateMOR).AddDays(a) && Convert.ToDateTime(x.CreateDate) <= Convert.ToDateTime(_startDateMOR).AddDays(a + 1) && Convert.ToDateTime(x.CreateDate) <= Convert.ToDateTime(_endDateMOR)).ToList();
                                    var sumOfTheKmOfDay = list.Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                    var mesaiDisiKm = 0;
                                    var bolgeDisiKm = 0;
                                    if (deviceArea != null)
                                    {
                                        if (area != null)
                                        {
                                            bolgeDisiKm = BolgeDisiYapilanKm(area, list);
                                        }
                                    }
                                    if (shift != null)
                                    {
                                        if (shift.Pazartesi == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 1)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000; 
                                        }
                                        if (shift.Sali == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 2)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                             
                                        }
                                        if (shift.Carsamba == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 3)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                            
                                        }
                                        if (shift.Persembe == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 4)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                            
                                        }
                                        if (shift.Cuma == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 5)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                            
                                        }
                                        if (shift.Cumartesi == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 6)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                             
                                        }
                                        if (shift.Pazar == true && (int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek == 7)
                                        {
                                            mesaiDisiKm = list.Where(x => Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) <= shiftStart || Convert.ToInt32(x.CreateDate.Split('T')[1].Split(':')[0]) >= shiftEnd).Sum(x => x.DistanceBetweenTwoPackages) / 1000;
                                             
                                        }
                                    }
                                    list1.Add(new MesafeOzetRaporuObjectRepo()
                                    {
                                        Arac = device.CarPlateNumber,
                                        Tarih = Convert.ToDateTime(_startDateMOR).AddDays(a).ToString("yyyy-MM-dd HH:mm:ss"),
                                        MesafeKm = sumOfTheKmOfDay,
                                        MesaiDisiKm = mesaiDisiKm,
                                        BolgeDisiMesafeKm = bolgeDisiKm,
                                        Gun = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[(int)Convert.ToDateTime(_startDateMOR).AddDays(a).DayOfWeek]
                                    });
                                }
                                dic.Add(keyClass, list1);
                            }
                            return dic;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
        public Dictionary<GGRKeyClass, List<AracGiseRaporuResultRepo>> AracGiseGecisRaporu(string _startDateGGR, string _endDateGGR, string[] _vehiclesGGR)
        {
            try
            {

                var tempList = new List<GiseGecisRaporuTemp>();
                var _startDate = _startDateGGR + " 00:00:00";
                var _endDate = _endDateGGR + " 23:59:59";
                var startDate = DateTime.ParseExact(_startDate, "yyyy-MM-dd HH:mm:ss", null).AddHours(-1);
                var endDate = DateTime.ParseExact(_endDate, "yyyy-MM-dd HH:mm:ss", null).AddHours(1);

                var list = new List<TollBooth>();


                using (var context = new VeraEntities())
                {
                    string[] deviceIds = _vehiclesGGR[0].Split(',');
                    var dic = new Dictionary<GGRKeyClass, List<AracGiseRaporuResultRepo>>();


                    list = context.TollBooth.ToList();
                    for (int a = 0; a < deviceIds.Length; a++)
                    {
                        var returnList = new List<AracGiseRaporuResultRepo>();
                        var _deviceId = Convert.ToInt32(deviceIds[a]);
                        var device = context.Device.FirstOrDefault(x => x.Id == _deviceId);


                        var groupDevice = context.GroupDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                        var grupName = "Grupsuz";
                        if (groupDevice != null)
                        {
                            grupName = context.Group.FirstOrDefault(x => x.Id == groupDevice.GroupId).Name;
                        }
                        var keyClass = new GGRKeyClass()
                        {
                            MobilTanimi = device.CarPlateNumber,
                            Grup = grupName
                        };

                        if (device.LastDeviceDataId == null)
                        {
                            var emptyObject = new AracGiseRaporuResultRepo();
                            returnList.Add(emptyObject);
                            dic.Add(keyClass, returnList);
                            continue;
                        }

                        var deviceDatas = context.DeviceData.Where(x => x.DeviceId == _deviceId &&
                                                                       x.CreateDate >= startDate &&
                                                                       x.CreateDate <= endDate
                                                                  ).OrderBy(x => x.CreateDate).ToList();
                        if (deviceDatas.Count == 0 || deviceDatas == null)
                            continue;
                        foreach (var deviceData in deviceDatas)
                        {
                            foreach (var item in list)
                            {
                                var latLonList = new List<CustomLatLon>();
                                var points = item.LatLons.Split(',');

                                for (var i = 0; i < points.Length; i += 2)
                                {
                                    var lat = Convert.ToDouble(points[i].Trim().Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                                    var lon = Convert.ToDouble(points[i + 1].Trim().Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                                    latLonList.Add(new CustomLatLon(lat, lon));
                                }

                                var result = Contains(new CustomLatLon((double)deviceData.Latitude, (double)deviceData.Longtitude), latLonList);

                                if (result)
                                {
                                    tempList.Add(new GiseGecisRaporuTemp()
                                    {
                                        DirectionDegree = deviceData.DirectionDegree,
                                        Gise = item.Name,
                                        Zaman = deviceData.CreateDate
                                    });
                                }
                            }
                        }


                        tempList = tempList.OrderBy(x => x.Zaman).ToList();

                        var dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(tempList.OrderBy(x => x.Zaman).ToList()));

                        var str = JsonConvert.SerializeObject(tempList);


                        dt.Columns.Add("WillBeDeleted");

                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            if ((i + 1) >= dt.Rows.Count)
                            {
                                break;
                            }
                            var currentGiseAdi = dt.Rows[i]["Gise"].ToString();
                            var nextGiseAdi = dt.Rows[i + 1]["Gise"].ToString();
                            var currentDegree = Convert.ToInt32(dt.Rows[i]["DirectionDegree"].ToString());
                            var nextDegree = Convert.ToInt32(dt.Rows[i + 1]["DirectionDegree"].ToString());

                            if (currentGiseAdi == nextGiseAdi)
                            {
                                if (currentDegree - 75 < nextDegree && currentDegree + 75 > nextDegree)
                                {
                                    dt.Rows[i + 1]["WillBeDeleted"] = "true";
                                }
                            }
                        }

                        for (var i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["WillBeDeleted"].ToString() == "true")
                            {
                                dt.Rows.RemoveAt(i);
                                i = -1;
                            }
                        }

                        var ss = JsonConvert.SerializeObject(dt);

                        dt.Columns.Remove("WillBeDeleted");
                        var dataRowsCount = (dt.Rows.Count % 2 == 1) ? dt.Rows.Count - 1 : dt.Rows.Count;
                        for (var i = 0; i < dataRowsCount; i += 2)
                        {
                            returnList.Add(new AracGiseRaporuResultRepo()
                            {
                                GirisDirectionDegree = Convert.ToDecimal(dt.Rows[i]["DirectionDegree"].ToString()),
                                GirisGisesi = dt.Rows[i]["Gise"].ToString(),
                                GirisZamani = Convert.ToDateTime(dt.Rows[i]["Zaman"].ToString()),
                                CikisDirectionDegree = Convert.ToDecimal(dt.Rows[i + 1]["DirectionDegree"].ToString()),
                                CikisGisesi = dt.Rows[i + 1]["Gise"].ToString(),
                                CikisZamani = Convert.ToDateTime(dt.Rows[i + 1]["Zaman"].ToString())
                            });

                        }

                        foreach (var item in returnList)
                        {
                            item.GecisSuresi = ReportDB.GetInstance().TotalHoursToHumanReadableString((item.CikisZamani - item.GirisZamani).TotalHours.ToString());
                        }
                        dic.Add(keyClass, returnList);
                    }
                    return dic;
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public Dictionary<BVARKeyClass, List<BolgeVarisAyrilisRaporuObject>> AracBolgeVarisAyrilisRaporu(string _startDateBVAR, string _endDateBVAR, int _companyAreaBVAR, string[] _vehiclesGGR, User _userObj)
        {
            try
            {
                var returnList = new List<BolgeVarisAyrilisRaporuObject>();
                var tempList = new List<BolgeVarisAyrilisRaporuObject>();

                var _startDate = _startDateBVAR + " 00:00:00";
                var _endDate = _endDateBVAR + " 23:59:59";

                var startDate = DateTime.ParseExact(_startDate, "yyyy-MM-dd HH:mm:ss", null).AddHours(-1);
                var endDate = DateTime.ParseExact(_endDate, "yyyy-MM-dd HH:mm:ss", null).AddHours(1);

                using (var context = new VeraEntities())
                {
                    string[] deviceIds = _vehiclesGGR[0].Split(',');
                    var dic = new Dictionary<BVARKeyClass, List<BolgeVarisAyrilisRaporuObject>>();
                    var areaList = new List<Area>();

                    if (_companyAreaBVAR == 0)
                    { //Tüm Bölgeler Seçiliyse
                        var company = CompanyUserDB.GetInstance().GetCompanyByCompanyUserId(_userObj);
                        var subCompanyIds = context.Company.Where(x => x.MainCompanyId == company.Id).Select(x => x.Id).ToList();
                        var areaIds = context.CompanyArea.Where(x => x.CompanyId == company.Id || subCompanyIds.Contains(x.CompanyId)).Select(x => x.AreaId).ToList();
                        areaList = context.Area.Where(x => areaIds.Contains(x.Id)).ToList();
                    }

                    if (_companyAreaBVAR != 0) //Tek Bölge Seçiliyse
                    {
                        areaList = context.Area.Where(x => x.Id == _companyAreaBVAR).ToList();
                    }



                    for (int a = 0; a < deviceIds.Length; a++)
                    {
                        var _deviceId = Convert.ToInt32(deviceIds[a]);
                        var device = context.Device.FirstOrDefault(x => x.Id == _deviceId);
                        var deviceDatas = context.DeviceData.Where(x => x.DeviceId == _deviceId &&
                                                                        x.CreateDate >= startDate &&
                                                                        x.CreateDate <= endDate
                                                                   ).OrderBy(x => x.CreateDate).ToList();
                        var groupDevice = context.GroupDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                        var grupName = "Grupsuz";
                        if (groupDevice != null)
                        {
                            grupName = context.Group.FirstOrDefault(x => x.Id == groupDevice.GroupId).Name;
                        }
                        var keyClass = new BVARKeyClass()
                        {
                            MobilTanimi = device.CarPlateNumber,
                            Grup = grupName
                        };

                        if (deviceDatas.Count == 0 || deviceDatas == null)
                        {
                            var emptyList = new List<BolgeVarisAyrilisRaporuObject>();
                            dic.Add(keyClass, emptyList);
                            continue;
                        }

                        var areaDeviceAreaIds = context.AreaDevice.Where(x => x.DeviceId == device.Id).Select(x => x.AreaId).ToList();
                        areaList = areaList.Where(x => areaDeviceAreaIds.Contains(x.Id)).ToList();

                        if (areaDeviceAreaIds.Count == 0 || areaDeviceAreaIds == null)
                        {
                            var emptyList = new List<BolgeVarisAyrilisRaporuObject>();
                            dic.Add(keyClass, emptyList);
                            continue;
                        }


                        bool? isInside = null;

                        foreach (var item in areaList)
                        {
                            var latLonList = new List<CustomLatLon>();
                            var points = item.LatsLongs.Split(',');
                            for (var i = 0; i < points.Length; i += 2)
                            {
                                var lat = Convert.ToDouble(points[i].Trim().Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                                var lon = Convert.ToDouble(points[i + 1].Trim().Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                                latLonList.Add(new CustomLatLon(lat, lon));
                            }
                            foreach (var deviceData in deviceDatas)
                            {
                                var result = Contains(new CustomLatLon((double)deviceData.Latitude, (double)deviceData.Longtitude), latLonList);

                                if (result)
                                {
                                    tempList.Add(new BolgeVarisAyrilisRaporuObject() //Giriş
                                    {
                                        AyrilisZamani = null,
                                        MesafeKm = deviceData.TotalKm.ToString(),
                                        VarisZamani = deviceData.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        Bolge = item.Name
                                    });
                                }

                                if (isInside != null && isInside == true && result == false) //Çıkış
                                {
                                    tempList.Add(new BolgeVarisAyrilisRaporuObject()
                                    {
                                        AyrilisZamani = deviceData.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                        MesafeKm = deviceData.TotalKm.ToString(),
                                        Bolge = item.Name
                                    });
                                    var sonAyrilisZamani = deviceData.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

                                    returnList.Add(new BolgeVarisAyrilisRaporuObject()
                                    {
                                        Bolge = item.Name,
                                        AyrilisZamani = tempList.LastOrDefault().AyrilisZamani,
                                        BeklemeDk = TotalHoursToHumanReadableString(Convert.ToString((Convert.ToDateTime(tempList.LastOrDefault().AyrilisZamani) - Convert.ToDateTime(tempList.FirstOrDefault(x => x.Bolge == item.Name && returnList.Count != 0 && x.VarisZamani != null && returnList.LastOrDefault().Bolge == item.Name ? Convert.ToDateTime(returnList.LastOrDefault(b => b.Bolge == item.Name).AyrilisZamani) <= Convert.ToDateTime(x.VarisZamani) : x.VarisZamani == tempList.FirstOrDefault(m => m.Bolge == item.Name).VarisZamani).VarisZamani)).TotalHours)),
                                        MesafeKm = Convert.ToString(Convert.ToInt32(tempList.LastOrDefault().MesafeKm) - Convert.ToInt32(tempList.FirstOrDefault(x => x.Bolge == item.Name && returnList.Count != 0 && x.VarisZamani != null && returnList.LastOrDefault().Bolge == item.Name ? Convert.ToDateTime(returnList.LastOrDefault(b => b.Bolge == item.Name).AyrilisZamani) <= Convert.ToDateTime(x.VarisZamani) : x.VarisZamani == tempList.FirstOrDefault(m => m.Bolge == item.Name).VarisZamani).MesafeKm)),
                                        VarisZamani = tempList.FirstOrDefault(x => x.Bolge == item.Name && returnList.Count != 0 && x.VarisZamani != null && returnList.LastOrDefault().Bolge == item.Name ? Convert.ToDateTime(returnList.LastOrDefault(b => b.Bolge == item.Name).AyrilisZamani) <= Convert.ToDateTime(x.VarisZamani) : x.VarisZamani == tempList.FirstOrDefault(m => m.Bolge == item.Name).VarisZamani).VarisZamani
                                    });
                                }
                                isInside = result;
                            }
                        }

                        dic.Add(keyClass, returnList);
                    }
                    return dic;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public Dictionary<KDRKeyClass, List<KontakDurumRaporuToShow>> KontakDurumRaporu(string _startDateKDR, string _endDateKDR, string[] _vehiclesKDR)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {

                            if (_startDateKDR.Length > 11 && _endDateKDR.Length > 11)
                            {
                                _startDateKDR = DateTime.ParseExact(_startDateKDR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                _endDateKDR = DateTime.ParseExact(_endDateKDR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            if (_startDateKDR.Length < 11 && _endDateKDR.Length < 11)
                            {
                                _startDateKDR += " 00:00:00";
                                _endDateKDR += " 23:59:59";
                                _startDateKDR = Convert.ToDateTime(_startDateKDR).ToString("dd.MM.yyyy HH:mm:ss");
                                _endDateKDR = Convert.ToDateTime(_endDateKDR).ToString("dd.MM.yyyy HH:mm:ss");
                                _startDateKDR = DateTime.ParseExact(_startDateKDR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                _endDateKDR = DateTime.ParseExact(_endDateKDR, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm:ss");
                            }

                            string[] deviceIds = _vehiclesKDR[0].Split(',');
                            var dic = new Dictionary<KDRKeyClass, List<KontakDurumRaporuToShow>>();

                            for (int i = 0; i < deviceIds.Length; i++)
                            {
                                var _deviceId = Convert.ToInt32(deviceIds[i]);
                                var device = context.Device.FirstOrDefault(x => x.Id == _deviceId);

                                var groupDevice = context.GroupDevice.FirstOrDefault(x => x.DeviceId == device.Id);
                                var grupName = "Grupsuz";
                                if (groupDevice != null)
                                {
                                    grupName = context.Group.FirstOrDefault(x => x.Id == groupDevice.GroupId).Name;
                                }
                                var keyClass = new KDRKeyClass()
                                {
                                    MobilTanimi = device.CarPlateNumber,
                                    Grup = grupName
                                };

                                if (device.LastDeviceDataId == null)
                                {
                                    var emptyObject = new KontakDurumRaporuToShow();
                                    var emptyList = new List<KontakDurumRaporuToShow>();
                                    emptyList.Add(emptyObject);
                                    dic.Add(keyClass, emptyList);
                                    continue;
                                }
                                cmd.CommandText = $"EXEC USP_SeferOzetiRaporu '{_startDateKDR }','{_endDateKDR }',{_deviceId}";


                                var reader = cmd.ExecuteReader();

                                StringBuilder sb = new StringBuilder();

                                while (reader.Read())
                                    sb.Append(reader.GetString(0));

                                reader.Close();

                                var json = sb.ToString();
                                var list2 = JsonConvert.DeserializeObject<List<SeferOzetiRaporuObject>>(json);
                                list2 = list2.Where(x => x.EndKontakAcilmaTarihi != null).ToList();
                                if (list2.Count == 0 || list2 == null)
                                {
                                    var emptyList = new List<KontakDurumRaporuToShow>();
                                    dic.Add(keyClass, emptyList);
                                    continue;
                                }
                                var returnKontakDurumuList = new List<KontakDurumRaporuToShow>();
                                foreach (var item in list2)
                                {

                                    returnKontakDurumuList.Add(new KontakDurumRaporuToShow()
                                    {
                                        Tarih = Convert.ToDateTime(item.StartKontakAcilmaTarihi).ToString("yyyy-MM-dd HH:mm:ss"),
                                        MobilTanimi = device.CarPlateNumber,
                                        KontakDurumu = "Kontak Açıldı",
                                        Enlem = item.StartLat,
                                        Boylam = item.StartLong,
                                        Konum = item.StartLocation,
                                    });
                                    returnKontakDurumuList.Add(new KontakDurumRaporuToShow()
                                    {
                                        Tarih = Convert.ToDateTime(item.EndKontakAcilmaTarihi).ToString("yyyy-MM-dd HH:mm:ss"),
                                        MobilTanimi = device.CarPlateNumber,
                                        KontakDurumu = "Kontak Kapandı",
                                        Enlem = item.EndLat,
                                        Boylam = item.EndLong,
                                        Konum = item.EndLocation
                                    });
                                }
                                returnKontakDurumuList = returnKontakDurumuList.Where(x => x.Konum != null).ToList();
                                dic.Add(keyClass, returnKontakDurumuList);

                            }
                            return dic;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
        public string TotalHoursToHumanReadableString(string _totalHours)
        {
            var totalHours = Convert.ToDouble(_totalHours.Replace(".", ","), CultureInfo.GetCultureInfo("tr-tr"));
            var totalMinutes = totalHours * 60;
            var totalMilliSeconds = TimeSpan.FromMinutes(totalMinutes).TotalMilliseconds;

            TimeSpan t = TimeSpan.FromMilliseconds(totalMilliSeconds);
            string answer = string.Format("{0:D2} Gün:{1:D2} Saat:{2:D2} Dakika:{3:D2} Saniye",
                                    t.Days,
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds);

            var duzgunTime = string.Join(" ", answer.Split(':').Where(x => Convert.ToInt32(x.Split(' ').First().Trim()) > 0));

            return duzgunTime;
        }
        public bool Contains(CustomLatLon location, List<CustomLatLon> _vertices)
        {
            //if (!Bounds.Contains(location))
            //    return false;

            var lastPoint = _vertices[_vertices.Count - 1];
            var isInside = false;
            var x = location.Longitude;

            foreach (var point in _vertices)
            {
                var x1 = lastPoint.Longitude;
                var x2 = point.Longitude;
                var dx = x2 - x1;

                if (Math.Abs(dx) > 180.0)
                {
                    // we have, most likely, just jumped the dateline (could do further validation to this effect if needed).  normalise the numbers.
                    if (x > 0)
                    {
                        while (x1 < 0)
                            x1 += 360;
                        while (x2 < 0)
                            x2 += 360;
                    }
                    else
                    {
                        while (x1 > 0)
                            x1 -= 360;
                        while (x2 > 0)
                            x2 -= 360;
                    }
                    dx = x2 - x1;
                }

                if ((x1 <= x && x2 > x) || (x1 >= x && x2 < x))
                {
                    var grad = (point.Latitude - lastPoint.Latitude) / dx;
                    var intersectAtLat = lastPoint.Latitude + ((x - x1) * grad);

                    if (intersectAtLat > location.Latitude)
                        isInside = !isInside;
                }
                lastPoint = point;
            }

            return isInside;
        }
        public string GetLocationFromLatLon(decimal _lat, decimal _long)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var location = context.LatLonLocation.FirstOrDefault(x => x.Latitude == _lat && x.Longtitude == _long).Location;
                    if (location != null)
                        return location;
                    return "";

                }
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
        public List<AverageAndMaxKm> GetAverageAndMaxKmFromDeviceDataList(int _deviceId, string _startDate, string _endDate)
        {
            using (var context = new VeraEntities())
            {
                using (var con = context.Database.GetDbConnection())
                {
                    try
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();

                        using (var cmd = con.CreateCommand())
                        {
                            cmd.CommandText = $"EXEC USP_GetAverageAndMaxKmFromDeviceDataList {_deviceId} ,'{_startDate}' ,'{_endDate}'";

                            var reader = cmd.ExecuteReader();

                            StringBuilder sb = new StringBuilder();

                            while (reader.Read())
                                sb.Append(reader.GetString(0));

                            var json = sb.ToString();
                            var list = JsonConvert.DeserializeObject<List<AverageAndMaxKm>>(json);
                            return list;
                        }
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
            }
        }
        public int BolgeDisiYapilanKm(Area _area, List<MesafeOzetRaporuObject> _deviceDataList)
        {
            try
            {
                var bolgeDisiYapilanKm = 0;
                var latLonList = new List<CustomLatLon>();
                var tempList = new List<BolgeVarisAyrilisRaporuObject>();
                var returnList = new List<BolgeVarisAyrilisRaporuObject>();
                var points = _area.LatsLongs.Split(',');
                bool? isInside = null;
                for (var i = 0; i < points.Length; i += 2)
                {
                    var lat = Convert.ToDouble(points[i].Trim().Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                    var lon = Convert.ToDouble(points[i + 1].Trim().Replace(".", ","), CultureInfo.GetCultureInfo("tr-TR"));
                    latLonList.Add(new CustomLatLon(lat, lon));
                }

                foreach (var deviceData in _deviceDataList)
                {
                    var result = Contains(new CustomLatLon((double)deviceData.Latitude, (double)deviceData.Longtitude), latLonList);
                    if (result)
                    {
                        tempList.Add(new BolgeVarisAyrilisRaporuObject() //Giriş
                        {
                            AyrilisZamani = null,
                            MesafeKm = deviceData.TotalKm.ToString(),
                            VarisZamani = Convert.ToDateTime(deviceData.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            Bolge = _area.Name
                        });
                    }

                    if (isInside != null && isInside == true && result == false) //Çıkış
                    {
                        tempList.Add(new BolgeVarisAyrilisRaporuObject()
                        {
                            AyrilisZamani = Convert.ToDateTime(deviceData.CreateDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            MesafeKm = deviceData.TotalKm.ToString(),
                            Bolge = _area.Name
                        });
                        var sonAyrilisZamani = Convert.ToDateTime(deviceData.CreateDate).ToString("yyyy-MM-dd HH:mm:ss");

                        returnList.Add(new BolgeVarisAyrilisRaporuObject()
                        {
                            Bolge = _area.Name,
                            AyrilisZamani = tempList.LastOrDefault().AyrilisZamani,
                            BeklemeDk = TotalHoursToHumanReadableString(Convert.ToString((Convert.ToDateTime(tempList.LastOrDefault().AyrilisZamani) - Convert.ToDateTime(tempList.FirstOrDefault(x => x.Bolge == _area.Name && returnList.Count != 0 && x.VarisZamani != null && returnList.LastOrDefault().Bolge == _area.Name ? Convert.ToDateTime(returnList.LastOrDefault(b => b.Bolge == _area.Name).AyrilisZamani) <= Convert.ToDateTime(x.VarisZamani) : x.VarisZamani == tempList.FirstOrDefault(m => m.Bolge == _area.Name).VarisZamani).VarisZamani)).TotalHours)),
                            MesafeKm = Convert.ToString(Convert.ToInt32(tempList.LastOrDefault().MesafeKm) - Convert.ToInt32(tempList.FirstOrDefault(x => x.Bolge == _area.Name && returnList.Count != 0 && x.VarisZamani != null && returnList.LastOrDefault().Bolge == _area.Name ? Convert.ToDateTime(returnList.LastOrDefault(b => b.Bolge == _area.Name).AyrilisZamani) <= Convert.ToDateTime(x.VarisZamani) : x.VarisZamani == tempList.FirstOrDefault(m => m.Bolge == _area.Name).VarisZamani).MesafeKm)),
                            VarisZamani = tempList.FirstOrDefault(x => x.Bolge == _area.Name && returnList.Count != 0 && x.VarisZamani != null && returnList.LastOrDefault().Bolge == _area.Name ? Convert.ToDateTime(returnList.LastOrDefault(b => b.Bolge == _area.Name).AyrilisZamani) <= Convert.ToDateTime(x.VarisZamani) : x.VarisZamani == tempList.FirstOrDefault(m => m.Bolge == _area.Name).VarisZamani).VarisZamani

                        });
                    }
                    isInside = result;
                }
                if (returnList.Count > 0 || returnList != null)
                {
                    bolgeDisiYapilanKm = returnList.Sum(x => Convert.ToInt32(x.MesafeKm));
                }
                return bolgeDisiYapilanKm;
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }
    }
    public class AverageAndMaxKm
    {
        public string AverageKm { get; set; }
        public string MaxKm { get; set; }
    }
    public class BolgeVarisAyrilisRaporuObject
    {
        public string VarisZamani { get; set; }
        public string AyrilisZamani { get; set; }
        public string BeklemeDk { get; set; }
        public string MesafeKm { get; set; }
        public string Bolge { get; set; }
    }
    public class BVARKeyClass
    {
        public string MobilTanimi { get; set; }
        public string Grup { get; set; }
    }
    public class SORKeyClass
    {
        public string MobilTanimi { get; set; }
        public string Grup { get; set; }
        public string GecenSure { get; set; }
        public string MaxKm { get; set; }
        public string AverageKm { get; set; }
    }
    public class SeferOzetiRaporuObject
    {
        public string StartDeviceDataId { get; set; }
        public string StartKontakAcilmaTarihi { get; set; }
        public string StartLat { get; set; }
        public string StartLong { get; set; }
        public string StartTotalKm { get; set; }
        public string StartLocation { get; set; }
        public string EndDeviceDataId { get; set; }
        public string EndKontakAcilmaTarihi { get; set; }
        public string EndLat { get; set; }
        public string EndLong { get; set; }
        public string EndTotalKm { get; set; }
        public string EndLocation { get; set; }
        public string AverageKm { get; set; }
        public string MaxSpeed { get; set; }

    }
    public class KontakDurumRaporuObject
    {
        public string StartKontakAcilmaTarihi { get; set; }
        public string StartLat { get; set; }
        public string StartLong { get; set; }
        public string StartTotalKm { get; set; }
        public string EndKontakAcilmaTarihi { get; set; }
        public string EndLat { get; set; }
        public string EndLong { get; set; }
        public string EndTotalKm { get; set; }
        public string AverageKm { get; set; }
        public string MaxSpeed { get; set; }
    }
    public class KontakDurumRaporuToShow
    {
        public string Tarih { get; set; }
        public string MobilTanimi { get; set; }
        public string KontakDurumu { get; set; }
        public string Konum { get; set; }
        public string Enlem { get; set; }
        public string Boylam { get; set; }
    }
    public class KDRKeyClass
    {
        public string Grup { get; set; }
        public string MobilTanimi { get; set; }
    }
    public class AracGiseRaporuResultRepo
    {
        public string GirisGisesi { get; set; }
        public string CikisGisesi { get; set; }
        public DateTime GirisZamani { get; set; }
        public DateTime CikisZamani { get; set; }
        public decimal GirisDirectionDegree { get; set; }
        public decimal CikisDirectionDegree { get; set; }
        public string GecisSuresi { get; set; }
    }
    public class GiseGecisRaporuTemp
    {
        public string Gise { get; set; }
        public DateTime Zaman { get; set; }
        public decimal DirectionDegree { get; set; }
    }
    public class GGRKeyClass
    {
        public string MobilTanimi { get; set; }
        public string Grup { get; set; }
    }
    public class BolgeGirisCikisRaporuTemp
    {
        public string Gise { get; set; }
        public DateTime Zaman { get; set; }
        public decimal DirectionDegree { get; set; }
    }
    public class SeferOzetiRaporuObjectRepo
    {
        public string Arac { get; set; }
        public string KalkisZamani { get; set; }
        public string VarisZamani { get; set; }
        public string KalkisPozisyon { get; set; }
        public string Sure { get; set; }
        public string Mesafe { get; set; }
        public string ToplamKm { get; set; }
        public string VarisPozisyon { get; set; }
    }
    public class GecmisKonumRaporuObject
    {
        public string Tarih { get; set; }
        public string Grup { get; set; }
        public string Arac { get; set; }
        public string Hiz { get; set; }
        public string Mesafe { get; set; }
        public string ToplamKm { get; set; }
        public string Enlem { get; set; }
        public string Boylam { get; set; }
        public string Konum { get; set; }
    }
    public class GKRKeyClass
    {
        public string MobilTanimi { get; set; }
        public string Grup { get; set; }
    }
    public class GecmisKonumRaporuObjectRepo : GecmisKonumRaporuObject
    {
    }

    public class SonKonumRaporuObject
    {
        public string Konum { get; set; }
        public string Grup { get; set; }
        public string Tarih { get; set; }
        public string MobilTanimi { get; set; }
        public string ToplamKm { get; set; }
    }
    public class SonKonumRaporuObjectRepo : SonKonumRaporuObject
    {
    }
    public class MesafeOzetRaporuObject
    {
        public string CreateDate { get; set; }
        public int DistanceBetweenTwoPackages { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
        public int TotalKm{ get; set; }
    }
    public class MesafeOzetRaporuObjectRepo : MesafeOzetRaporuObject
    {
        public string Arac { get; set; }
        public string Tarih { get; set; }
        public decimal MesafeKm { get; set; }
        public decimal MesaiDisiKm { get; set; }
        public decimal BolgeDisiMesafeKm { get; set; }
        public string Gun { get; set; }

    }
    public class MORKeyClass
    {
        public string MobilTanimi { get; set; }
        public string Grup { get; set; }
        public string Bolge { get; set; }
    }
    public class DeviceInfoForMap
    {
        public decimal lat { get; set; }
        public decimal lng { get; set; }
        public string CarPlateNumber { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public decimal Altitude { get; set; }
        public string IoStatus { get; set; }
        public decimal TotaLkm { get; set; }
        public int KmPerHour { get; set; }
        public decimal DirectionDegree { get; set; }
        public int DeviceId { get; set; }
        public int ActivityTime { get; set; }
        public string LastDeviceDataCreateDate { get; set; }
    }
    public class CustomLatLon
    {
        public CustomLatLon(double _lat, double _lon)
        {
            this.Latitude = _lat;
            this.Longitude = _lon;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Giseler
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LatLons { get; set; }
    }
}
