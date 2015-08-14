using System.Collections.Generic;
using System.Data;
using DAO;
using EntityModele.Criteres;
using EntityModele;
using DAO.Cache;
using System;

namespace BIZ
{
    public class BizClient
    {
        private DaoClient daoClient;

        public DataTable GetClientByNumero(int Numero)
        {
            daoClient = new DaoClient();
            var Params = new List<Filtre>(
                new[] {
                        new Filtre { Name = "@NumeroClient", Value = Numero},
                });
            var dt = daoClient.PsSelect(Params);
            return dt;
        }

        public DataTable GetAllDt()
        {
            daoClient = new DaoClient();
            var dt = daoClient.PsSelect(new List<Filtre>());
            return dt;
        }

        public List<ClientEntity> GetAll()
        {
            daoClient = new DaoClient();
            var dt = CacheManager.Current.GetEntities<ClientEntity>();
            if (dt == null)
            {
                dt = daoClient.PsSelectEntities();
                CacheManager.Current.SetEntities<ClientEntity>(dt);
            }
            return dt;
        }

        public List<ClientEntity> GetByCriteres(List<Filtre> filtre)
        {
            daoClient = new DaoClient();
            var listClients = daoClient.PsSelectEntities(filtre);
            return listClients;
        }

    }
}