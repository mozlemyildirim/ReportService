using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public  class SettingsDB
    {
        private static SettingsDB instance;
        public static SettingsDB GetInstance()
        {
            if (instance == null)
            {
                instance = new SettingsDB();
            }
            return instance;
        }
        public Settings AddNewSettings(Settings _Settings)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Settings.Add(_Settings);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Settings : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Settings> GetAllSettingss()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var SettingsList = context.Settings.ToList();
                    return SettingsList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Settings GetSettingsById(int _SettingsId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Settings = context.Settings.FirstOrDefault(a => a.Id == _SettingsId);
                    return Settings != null ? Settings : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteSettings(int _SettingsId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Settings = context.Settings.FirstOrDefault(a => a.Id == _SettingsId);
                    if (Settings != null)
                    {
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
        private void UpdateObject(Settings _newSettings, ref Settings _oldSettings)
        {
            try
            {

                foreach (PropertyInfo SettingsPropInfo in _newSettings.GetType().GetProperties().ToList())
                {
                    _oldSettings.GetType().GetProperty(SettingsPropInfo.Name).SetValue(_oldSettings, _newSettings.GetType().GetProperty(SettingsPropInfo.Name).GetValue(_newSettings));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Settings UpdateSettings(Settings _Settings)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldSettings = context.Settings.FirstOrDefault(u => u.Id == _Settings.Id);
                    if (oldSettings != null)
                    {
                        UpdateObject(_Settings, ref oldSettings);
                        var numberOfUpdatedSettings = context.SaveChanges();
                        return numberOfUpdatedSettings > 0 ? _Settings : null;
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
    }
}
