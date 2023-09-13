using DataBase;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClientService
    {
        #region singleton
        public static ClientService Instance
        {
            get
            {
                if (instance == null) instance = new ClientService();
                return instance;
            }
        }
        private static ClientService instance { get; set; }

        public ClientService()
        {
        }
        #endregion

        public IEnumerable<Client> GetClients()
        {
            using(var context = new ApplicationDbContext())
            {
                var data = context.Client.OrderBy(x=>x.PreferNo).ToList();
                return data;
            }
        }

        public string SaveClient(Client client)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                client.CreatedOn = HelperService.Instance.getCurrentIST();
                client.IsActive = true;
                context.Client.Add(client);
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public string EditClient(Client client)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                context.Entry(client).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
        public string DeleteClient(int Id)
        {
            var result = "false";
            using (var context = new ApplicationDbContext())
            {
                var data = context.Client.FirstOrDefault(x => x.Id == Id);
                context.Client.Remove(data);
                context.SaveChanges();
                result = "true";
            }
            return result;
        }
    }
}
