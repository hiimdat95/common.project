using Domain.Entities;
using Infrastructure.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class QueryMigrationInitilize
    {
        private readonly AppDbContext _context;

        public QueryMigrationInitilize(AppDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            var queryNewestIndb = _context.QueryMigrations.OrderByDescending(x => x.MigrationDate).FirstOrDefault();
            DirectoryInfo d = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory() + @"\Queries"); //Assuming Test is your Folder

            FileInfo[] files = d.GetFiles("*.sql"); //Getting sql files
            if (queryNewestIndb != null)
            {
                List<string> lstFileName = files.Select(x => x.Name.Replace(".sql", string.Empty)).Where(q => Int64.Parse(q) > Int64.Parse(queryNewestIndb.Name)).ToList();
                files = files.Where(f => lstFileName.Contains(f.Name.Replace(".sql", string.Empty))).ToArray();
            }

            foreach (FileInfo file in files)
            {
                var query = System.IO.File.ReadAllText(file.FullName);
                var cnn = (SqlConnection)_context.Database.GetDbConnection();
                if (cnn.State == ConnectionState.Closed)
                    cnn.Open();
                using (var cmd = new SqlCommand(query, cnn))
                using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult)) ;
                _context.QueryMigrations.Add(new QueryMigration
                {
                    Name = file.Name.Replace(".sql", string.Empty)
                });
                _context.SaveChanges();
            }
        }
    }
}