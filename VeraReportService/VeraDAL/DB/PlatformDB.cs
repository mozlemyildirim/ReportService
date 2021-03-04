using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class PlatformDB
	{
		private static PlatformDB instance;
		public static PlatformDB GetInstance()
		{
			if (instance == null)
			{
				instance = new PlatformDB();
			}
			return instance;
		}
		public Platform AddNewPlatform(Platform _Platform)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Platform.Add(_Platform);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _Platform : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<Platform> GetAllPlatforms()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var PlatformList = context.Platform.ToList();
					return PlatformList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Platform GetPlatformById(int _PlatformId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Platform = context.Platform.FirstOrDefault(a => a.Id == _PlatformId && a.Status == 1);
					return Platform != null ? Platform : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeletePlatform(int _PlatformId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Platform = context.Platform.FirstOrDefault(a => a.Id == _PlatformId);
					if (Platform != null)
					{
						Platform.Status = 0;
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
		private void UpdateObject(Platform _newPlatform, ref Platform _oldPlatform)
		{
			try
			{

				foreach (PropertyInfo PlatformPropInfo in _newPlatform.GetType().GetProperties().ToList())
				{
					_oldPlatform.GetType().GetProperty(PlatformPropInfo.Name).SetValue(_oldPlatform, _newPlatform.GetType().GetProperty(PlatformPropInfo.Name).GetValue(_newPlatform));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Platform UpdatePlatform(Platform _Platform)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldPlatform = context.Platform.FirstOrDefault(u => u.Id == _Platform.Id);
					if (oldPlatform != null)
					{
						UpdateObject(_Platform, ref oldPlatform);
						var numberOfUpdatedPlatform = context.SaveChanges();
						return numberOfUpdatedPlatform > 0 ? _Platform : null;
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
		public Platform GetPlatformByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Platform = context.Platform.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (Platform == null && _name!=null)
					{
						Platform = new Platform()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.Platform.Add(Platform);
						context.SaveChanges();
						return Platform;
					}
                    return Platform;
				
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
