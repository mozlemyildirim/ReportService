using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VeraDAL.Entities;

namespace VeraDAL.DB
{
	public class SectorDB
	{
		private static SectorDB instance;
		public static SectorDB GetInstance()
		{
			if (instance == null)
			{
				instance = new SectorDB();
			}
			return instance;
		}
		public Sector AddNewSector(Sector _Sector)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					context.Sector.Add(_Sector);
					int numOfInserted = context.SaveChanges();
					return numOfInserted > 0 ? _Sector : null;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public List<Sector> GetAllSectors()
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var SectorList = context.Sector.Where(a=>a.Status==1).ToList();
					return SectorList;
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Sector GetSectorById(int _SectorId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Sector = context.Sector.FirstOrDefault(a => a.Id == _SectorId && a.Status == 1);
					return Sector != null ? Sector : null;
				}
			}
			catch (Exception)
			{

				throw;
			}

		}
		public bool DeleteSector(int _SectorId)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Sector = context.Sector.FirstOrDefault(a => a.Id == _SectorId);
					if (Sector != null)
					{
						Sector.Status = 0;
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
		private void UpdateObject(Sector _newSector, ref Sector _oldSector)
		{
			try
			{

				foreach (PropertyInfo SectorPropInfo in _newSector.GetType().GetProperties().ToList())
				{
					_oldSector.GetType().GetProperty(SectorPropInfo.Name).SetValue(_oldSector, _newSector.GetType().GetProperty(SectorPropInfo.Name).GetValue(_newSector));
				}


			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}
		public Sector UpdateSector(Sector _Sector)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var oldSector = context.Sector.FirstOrDefault(u => u.Id == _Sector.Id);
					if (oldSector != null)
					{
						UpdateObject(_Sector, ref oldSector);
						var numberOfUpdatedSector = context.SaveChanges();
						return numberOfUpdatedSector > 0 ? _Sector : null;
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
		public Sector GetSectorByNameOrInsert(string _name)
		{
			try
			{
				using (var context = new VeraEntities())
				{
					var Sector = context.Sector.FirstOrDefault(x => x.Name.Trim() == _name.Trim() && x.Status > 0);

					if (Sector == null)
					{
						Sector = new Sector()
						{
							Name = _name.Trim(),
							Status = 1
						};

						context.Sector.Add(Sector);
						context.SaveChanges();
						return Sector;
					}

					return Sector;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}
	}
}
