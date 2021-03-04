using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
    public class ShiftDB
    {
        private static ShiftDB instance;
        public static ShiftDB GetInstance()
        {
            if (instance == null)
            {
                instance = new ShiftDB();
            }
            return instance;
        }
        public Shift AddNewShift(Shift _Shift)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    context.Shift.Add(_Shift);
                    int numOfInserted = context.SaveChanges();
                    return numOfInserted > 0 ? _Shift : null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<Shift> GetAllShiftes()
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var ShiftList = context.Shift.ToList();
                    return ShiftList;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Shift GetShiftById(int _ShiftId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Shift = context.Shift.FirstOrDefault(a => a.Id == _ShiftId && a.Status == 1);
                    return Shift != null ? Shift : null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool DeleteShift(int _ShiftId)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var Shift = context.Shift.FirstOrDefault(a => a.Id == _ShiftId);
                    if (Shift != null)
                    {
                        Shift.Status = 0;
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
        private void UpdateObject(Shift _newShift, ref Shift _oldShift)
        {
            try
            {

                foreach (PropertyInfo ShiftPropInfo in _newShift.GetType().GetProperties().ToList())
                {
                    _oldShift.GetType().GetProperty(ShiftPropInfo.Name).SetValue(_oldShift, _newShift.GetType().GetProperty(ShiftPropInfo.Name).GetValue(_newShift));
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public Shift UpdateShift(Shift _Shift)
        {
            try
            {
                using (var context = new VeraEntities())
                {
                    var oldShift = context.Shift.FirstOrDefault(u => u.Id == _Shift.Id);
                    if (oldShift != null)
                    {
                        UpdateObject(_Shift, ref oldShift);
                        var numberOfUpdatedShift = context.SaveChanges();
                        return numberOfUpdatedShift > 0 ? _Shift : null;
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
