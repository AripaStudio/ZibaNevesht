using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace ZibaNevesht.Services
{
    [Table("ZBchamekade")]
    public class ZBchamekade
    {
        [Key]
        public int ChameID { get; set; }

        public string PoetName { get; set; }

        public string CategoryName { get; set; }

        public string ChameText { get; set; }

    }
    [Table("ZBmishmare")]
    public class ZBmishmare
    {
        [Key]
        public string Name { get; set; }

        public int NumberOfUsedWord { get; set; }

    }

    public enum AlertLevel
    {
        White = 0,
        Orange = 1,
        Red = 2
    }
    [Table("ZBgoshBeZang")]
    public class ZBgoshBeZang
    {
        [Key]
        public string NameWord { get; set; }

        public AlertLevel AlertLevel { get; set; }

    }

    public class DBmanager : DbContext
    {
        public DbSet<ZBchamekade> ZBchamekades { get; set; } 

        public DbSet<ZBgoshBeZang> ZBgoshBeZangs { get; set; }

        public DbSet<ZBmishmare> ZBmishmares { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=.\Assets\Database\ZibaNeveshChakameDataBase.db");
        }
    }


    public static class ZibaNeveshtDBmanager
    {
        public static List<ZBchamekade> GettingAllPoemsOfAPoet(string nameOfPoet)
        {

            using var db = new DBmanager();
            return db.ZBchamekades.AsNoTracking()
                .Where(poem => poem.PoetName == nameOfPoet)
                .ToList();
        }


        public static List<string> GettingAllTextOfPoemsOfAPoet(string nameOfPoet)
        {
            using var db = new DBmanager();
            return db.ZBchamekades.AsNoTracking()
                .Where(poem => poem.PoetName == nameOfPoet)
                .Select(s => s.ChameText)
                .ToList();
        }

        public static async Task<List<string>> GettingRandomTextOfPoemsOfAPoet(string nameOfPoet, int numberOfRsl = 20)
        {

            using var db = new DBmanager();
            var poems = await db.ZBchamekades
                .Where(x => x.PoetName == nameOfPoet)
                .Select(x => x.ChameText)
                .ToListAsync();
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(poems));
            return poems.Take(numberOfRsl).ToList();

            //var records = db.ZBchamekades
            //    .Count(s => s.PoetName == nameOfPoet);
            //int randomNumber = 0;
            //if (records >= 1)
            //{
            //    Random rand = new Random();
            //    randomNumber = rand.Next(1, records);
            //}
            //else
            //{
            //    randomNumber = records;
            //}
            //return db.ZBchamekades.AsNoTracking()
            //    .Where(poem => poem.PoetName == nameOfPoet)
            //    .Select(s => s.ChameText)
            //    .OrderBy(o => randomNumber.ToString())
            //    .Take(numberOfRsl)
            //    .ToList();
        }
    }

    public interface IMishmareDBmanager
    {
        public Task<List<ZBmishmare>> GetAllMishmare();

        public Task<ZBmishmare> GetOneMishmare(string name);
        public Task AddOrUpdate(string input);

        public Task AddOrUpdate(string input, int usageCount);


        public Task Remove(string input);

        public Task RemoveAll();

        public Task RemoveAllExceptForTop10();
    }

    public interface IGoshBeZang
    {
        public Task<List<ZBgoshBeZang>> GetAllGoshBeZangNames();

        public Task<bool> NameExists(string name);

        public Task<AlertLevel?> GetAlertLevel(string name);

        public Task AddOrUpdate(string input, AlertLevel alertLevel);


        public Task Remove(string input);

        public Task RemoveAll();
    }
    public class MishmareDBmanager : IMishmareDBmanager
    {
        public async Task<List<ZBmishmare>> GetAllMishmare()
        {
            await using var db = new DBmanager();
            try
            {
                var rsl = await db.ZBmishmares.AsNoTracking()
                    .ToListAsync();
                return rsl;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new List<ZBmishmare>();
            }

        }

        public async Task<ZBmishmare> GetOneMishmare(string name)
        {
            await using var db = new DBmanager();
            ZBmishmare? rsl = new ZBmishmare();
            try
            {
                rsl = await db.ZBmishmares.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == name);
                if (rsl != null) return rsl;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return rsl;
        }

        public async Task AddOrUpdate(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            var inputToLower = input.ToLower(CultureInfo.InvariantCulture);
            await using (var db = new DBmanager())
            {
                var newMishmare = new ZBmishmare();
                if (await db.ZBmishmares.AnyAsync(z => z.Name == inputToLower))
                {
                    newMishmare = await db.ZBmishmares.FirstOrDefaultAsync(f => f.Name == inputToLower);
                    if (newMishmare != null)
                    {
                        newMishmare.NumberOfUsedWord += 1;
                        db.ZBmishmares.Update(newMishmare);
                    }
                }
                else
                {
                    newMishmare.Name = inputToLower;
                    newMishmare.NumberOfUsedWord = 1;
                    await db.ZBmishmares.AddAsync(newMishmare);
                }
                await db.SaveChangesAsync();
            }



        }

        public async Task AddOrUpdate(string input, int usageCount)
        {
            if (string.IsNullOrEmpty(input)) return;
            var inputToLower = input.ToLower(CultureInfo.InvariantCulture);
            await using (var db = new DBmanager())
            {
                var newMishmare = new ZBmishmare();
                if (await db.ZBmishmares.AnyAsync(z => z.Name == inputToLower))
                {
                    newMishmare = await db.ZBmishmares.FirstOrDefaultAsync(f => f.Name == inputToLower);
                    if (newMishmare != null)
                    {
                        newMishmare.NumberOfUsedWord += usageCount;
                        db.ZBmishmares.Update(newMishmare);
                    }
                }
                else
                {
                    newMishmare.Name = inputToLower;
                    newMishmare.NumberOfUsedWord = usageCount;
                    await db.ZBmishmares.AddAsync(newMishmare);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task Remove(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            await using var db = new DBmanager();
            if (await db.ZBmishmares.AnyAsync(z => z.Name == input))
            {
                var get = await db.ZBmishmares.FirstOrDefaultAsync(s => s.Name == input);
                if (get != null)
                {
                    db.ZBmishmares.Remove(get);
                    await db.SaveChangesAsync();
                }

            }



        }

        public async Task RemoveAll()
        {
            await using var db = new DBmanager();
            try
            {
                var getAll = await db.ZBmishmares.ToListAsync();
                if (getAll.Count == 0) return;
                db.ZBmishmares.RemoveRange(getAll);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task RemoveAllExceptForTop10()
        {
            await using var db = new DBmanager();
            try
            {
                var getAll = await db.ZBmishmares.ToListAsync();
                List<ZBmishmare> topTen = getAll.OrderByDescending(x => x.NumberOfUsedWord).Take(10).ToList();
                var keepNames = topTen.Select(s => s.Name).ToHashSet();
                var toRemove = getAll.Where(x => !keepNames.Contains(x.Name)).ToList();

                if (toRemove.Count == 0) return;

                db.ZBmishmares.RemoveRange(toRemove);
                await db.SaveChangesAsync();


            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }




        }



    }

    public class GoshBeZang : IGoshBeZang
    {
        public async Task<List<ZBgoshBeZang>> GetAllGoshBeZangNames()
        {
            await using var db = new DBmanager();
            return await db.ZBgoshBeZangs.AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> NameExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            { return false; }

            await using var db = new DBmanager();
            return await db.ZBmishmares.AsNoTracking()
                .AnyAsync(x => x.Name == name);

        }

        public async Task<AlertLevel?> GetAlertLevel(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            if (await NameExists(name))
            {
                await using var db = new DBmanager();
                var getData = await db.ZBgoshBeZangs.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.NameWord == name);
                if (getData != null)
                {
                    return getData.AlertLevel;
                }


            }

            return null;
        }

        public async Task AddOrUpdate(string input, AlertLevel alertLevel)
        {

            if (string.IsNullOrEmpty(input)) return;
            var inputLower = input.ToLower(CultureInfo.InvariantCulture);
            await using (var db = new DBmanager())
            {
                var newGoshBeZang = new ZBgoshBeZang();
                if (await db.ZBgoshBeZangs.AnyAsync(z => z.NameWord == inputLower))
                {
                    return;
                }


                newGoshBeZang.NameWord = inputLower;
                newGoshBeZang.AlertLevel = alertLevel;
                await db.ZBgoshBeZangs.AddAsync(newGoshBeZang);
                await db.SaveChangesAsync();


            }
        }

        public async Task Remove(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            await using var db = new DBmanager();
            if (await db.ZBgoshBeZangs.AnyAsync(z => z.NameWord == input))
            {
                var get = await db.ZBgoshBeZangs.FirstOrDefaultAsync(s => s.NameWord == input);
                if (get != null)
                {
                    db.ZBgoshBeZangs.Remove(get);
                    await db.SaveChangesAsync();
                }

            }
        }

        public async Task RemoveAll()
        {

            await using var db = new DBmanager();
            if (await db.ZBgoshBeZangs.CountAsync() == 0) return;
            var remove = await db.ZBgoshBeZangs.ToListAsync();
            db.ZBgoshBeZangs.RemoveRange(remove);
            await db.SaveChangesAsync();
        }
    }
}
