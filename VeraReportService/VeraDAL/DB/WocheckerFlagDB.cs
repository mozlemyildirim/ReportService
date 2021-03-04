using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class WocheckerFlagDB
	{
		private static WocheckerFlagDB instance;
		public static WocheckerFlagDB GetInstance()
		{
			if (instance == null)
			{
				instance = new WocheckerFlagDB();
			}
			return instance;
		}
		public WocheckerFlag AddNewWocheckerFlag(WocheckerFlag _WocheckerFlag)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.WocheckerFlag.Add(_WocheckerFlag);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _WocheckerFlag : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<WocheckerFlag> GetAllWocheckerFlags()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var WocheckerFlagList = context.WocheckerFlag.ToList();
					return WocheckerFlagList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public WocheckerFlag GetWocheckerFlagById(int _WocheckerFlagId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var WocheckerFlag = context.WocheckerFlag.FirstOrDefault(a => a.Id == _WocheckerFlagId && a.Status == 1);
					return WocheckerFlag != null ? WocheckerFlag : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteWocheckerFlag(int _WocheckerFlagId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var WocheckerFlag = context.WocheckerFlag.FirstOrDefault(a => a.Id == _WocheckerFlagId);
					if (WocheckerFlag != null)
					{
						WocheckerFlag.Status = 0;
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
		private void UpdateObject(WocheckerFlag _newWocheckerFlag, ref WocheckerFlag _oldWocheckerFlag)
		{
			try
			{

				foreach (PropertyInfo WocheckerFlagPropInfo in _newWocheckerFlag.GetType().GetProperties().ToList())
				{
					_oldWocheckerFlag.GetType().GetProperty(WocheckerFlagPropInfo.Name).SetValue(_oldWocheckerFlag, _newWocheckerFlag.GetType().GetProperty(WocheckerFlagPropInfo.Name).GetValue(_newWocheckerFlag));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public WocheckerFlag UpdateWocheckerFlag(WocheckerFlag _WocheckerFlag)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldWocheckerFlag = context.WocheckerFlag.FirstOrDefault(u => u.Id == _WocheckerFlag.Id);
					if (oldWocheckerFlag != null)
					{
						UpdateObject(_WocheckerFlag, ref oldWocheckerFlag);
						var numberOfUpdatedWocheckerFlag = context.SaveChanges();
						return numberOfUpdatedWocheckerFlag > 0 ? _WocheckerFlag : null;
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

		public WocheckerFlag GetWocheckerFlagByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var WocheckerFlag = context.WocheckerFlag.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (WocheckerFlag == null)
					{
						WocheckerFlag = new WocheckerFlag()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.WocheckerFlag.Add(WocheckerFlag);
						context.SaveChanges();
						return WocheckerFlag;
					}

					return WocheckerFlag;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
	
